using System.Collections;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    [SerializeField] protected Transform _stackTransform;
    [SerializeField] protected SphereCollider _interactHitbox;
    public Monkey stack;
    private CharacterHandler _charHandler;

    public event System.Action OnSwitchEvent;

    private void Start()
    {
        stack = null;
        _charHandler = transform.GetComponentInParent<CharacterHandler>();
    }

    public Monkey GetInteractedMonkey()
    {
        Monkey foundMonkey = null;
        var colliders = Physics.OverlapSphere(transform.position, _interactHitbox.radius, LayerMask.GetMask("Player"));
        float closest = 999.0f;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Monkey>() == this) continue;
            if (!colliders[i].GetComponent<MovementController>().enabled) continue;
            float dist = (transform.position - colliders[i].transform.position).magnitude;
            if (dist < closest)
            {
                closest = dist;
                foundMonkey = colliders[i].GetComponent<Monkey>();
            }
        }
        return foundMonkey;
    }

    public void ApplyStack(Monkey given)
    {
        stack = given;
        stack.GetComponent<MovementController>().enabled = false;
        stack.transform.SetParent(_stackTransform, false);
        stack.transform.localPosition = Vector3.zero;
    }
    public void RevertStack()
    {
        stack.GetComponent<MovementController>().enabled = true;
        stack.transform.SetParent(_charHandler.transform, true);
        stack.GetComponent<CharacterController>().enabled = false;

        // NOTE: CAST BEFORE PUTTING
        stack.transform.position = stack.transform.position + new Vector3(2,0,0);

        stack.GetComponent<CharacterController>().enabled = true;
        stack = null;
    }

    public void AddToStack(Monkey given) {
        Monkey selected = this;
        Monkey stackSlot = selected.stack;
        if (stackSlot != null)
        {
            while (stackSlot != null)
            {
                selected = stackSlot;
                stackSlot = selected.stack;
            }
        }
        selected.ApplyStack(given);
    }
    public void RemoveFromStack()
    {
        Monkey selected = this;
        Monkey stackSlot = selected.stack;
        if (stackSlot != null)
        {
            while (stackSlot.stack != null)
            {
                selected = stackSlot;
                stackSlot = selected.stack;
            }
            selected.RevertStack();
        }
    }

    public virtual void OnSwitch()
    {
        OnSwitchEvent?.Invoke();
    }
}
