using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    CanvasGroup _interactableUI;

    [SerializeField]
    Image _buttonImage;

    [SerializeField]
    InputActionAsset _controls;

    [SerializeField]
    UnityEvent OnInteractedEvent;

    InputAction _action;

    private void Awake()
    {
        _action = _controls["Interact"];
        _interactableUI.gameObject.SetActive(false);

        if (!_buttonImage)
        {
            _buttonImage = _interactableUI.GetComponentInChildren<Image>();
        }
    }

    public void onInteract(Transform player)
    {
        if (_action.WasPressedThisFrame())
        {
            OnInteractedEvent?.Invoke();
        }

        if (_action.IsPressed())
            _buttonImage.color = Color.yellow;
        else
            _buttonImage.color = Color.white;
    }

    public void onEnterProximity(Transform player)
    {
        _interactableUI.DOKill();
        _interactableUI.gameObject.SetActive(true);
        _interactableUI.DOFade(1, 1f);
    }

    public void onExitProximity(Transform player)
    {
        //disableTalking();
        _interactableUI.DOFade(0, 0.5f).onComplete += () => {
            _interactableUI.gameObject.SetActive(false);
        };
    }

    //IEnumerator setInteractColorToYellow()
    //{
    //    _buttonImage.color = Color.yellow;
    //    yield return new WaitForSeconds(0.2f);
    //    _buttonImage.color = Color.white;
    //}



    //public void setStartDialogueNode(DialogueNode node)
    //{
    //    _dialogueNodeStart = node;
    //}

}
