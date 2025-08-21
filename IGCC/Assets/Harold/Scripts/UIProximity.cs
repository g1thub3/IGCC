using UnityEngine;
using System;
using DG.Tweening;

public class UIProximity : MonoBehaviour
{
    [SerializeField]
    float _interactRadius = 3;

    [SerializeField]
    LayerMask _playerMask;


    [SerializeField]
    CanvasGroup _interactableUI;

    ////If the Dialogue Initiator has a quest to give
    //QuestGiver _questGiver;



    Transform _target;

    public event System.Action OnTalkable;
    public event System.Action OnUnTalkable;

    //bool hasSpoken = false;
    bool _canInteract = true;


    private void Start()
    {
        _interactableUI.alpha = 0f;
        _interactableUI.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _interactableUI.DOKill();
    }

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _playerMask);

        //If we hit something
        if (colliders.Length > 0 && !_canInteract)
        {
            inInteractRange();
        }
        else if (_canInteract && colliders.Length <= 0)
            exitInteractRange();

        if (_canInteract)
        {

        }
    }


    //public virtual void startDialogue()
    //{
    //    DialogueManager.Instance.startNode(_dialogueNodeStart);
    //}

    private void inInteractRange()
    {
        enableTalking();

        _interactableUI.DOKill();
        _interactableUI.gameObject.SetActive(true);
        _interactableUI.DOFade(1, 1f);
    }

    private void exitInteractRange()
    {
        disableTalking();
        _interactableUI.DOFade(0, 0.5f).onComplete += () => {
            _interactableUI.gameObject.SetActive(false);
        };
    }

    void enableTalking()
    {
        _canInteract = true;
        OnTalkable?.Invoke();
    }

    void disableTalking()
    {
        _canInteract = false;
        OnUnTalkable?.Invoke();
    }

    //public void setStartDialogueNode(DialogueNode node)
    //{
    //    _dialogueNodeStart = node;
    //}

}
