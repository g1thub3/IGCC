using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    float _interactRadius = 5f;

    [SerializeField]
    LayerMask _mask;

    IInteractable _currentInteractable;

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _mask);

        //If we hit something
        if (colliders.Length > 0 && _currentInteractable == null)
        {
            _currentInteractable = colliders[0].GetComponent<IInteractable>();

            if (_currentInteractable != null)
                _currentInteractable.onEnterProximity(transform);
        }
        else if (_currentInteractable != null && colliders.Length <= 0) {
            _currentInteractable.onExitProximity(transform);
            _currentInteractable = null;
        }

        //
        if (_currentInteractable != null)
        {
           _currentInteractable.onInteract(transform);
        }
    }

    public void OnSwitchCharacter()
    {
        _currentInteractable.onExitProximity(transform);
        _currentInteractable = null;
    }


}
