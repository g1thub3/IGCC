using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    Tween _tween;

    private void Awake()
    {
        _action = _controls["Interact"];
        _interactableUI.gameObject.SetActive(false);

        if (!_buttonImage)
        {
            _buttonImage = _interactableUI.GetComponentInChildren<Image>();
        }
    }

    public void OnDisable()
    {
        _buttonImage.DOKill();
        _tween.Kill();
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
        if (gameObject.IsDestroyed())
            return;
        _interactableUI.DOKill();
        _interactableUI.gameObject.SetActive(true);
        _tween = _interactableUI.DOFade(1, 1f);
    }

    public void onExitProximity(Transform player)
    {
        if (gameObject.IsDestroyed())
            return;
            //disableTalking();
        _tween = _interactableUI.DOFade(0, 0.5f);
        _tween.onComplete += () => {

            //Debug.Log("Complete is called");
            if (!gameObject.IsDestroyed())
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
