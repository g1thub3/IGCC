using UnityEngine;
using UnityEngine.InputSystem;

public class GreenMonkey : Monkey
{
    private GameObject _carrying;

    public bool IsCarrying {
        get { return _carrying != null;}
    }

    protected new void Start()
    {
        base.Start();
        _carrying = null;
        index = 1;
    }

    private void ApplyCarry()
    {
        if (_carrying.TryGetComponent<MovementController>(out MovementController controller))
        {
            controller.enabled = false;
        }
        _carrying.transform.SetParent(_stackTransform);
        _carrying.transform.localPosition = Vector3.zero;
    }
    private void RevertCarry()
    {
        if (_carrying.TryGetComponent<MovementController>(out MovementController controller))
        {
            controller.enabled = true;
            _carrying.transform.SetParent(_charHandler.transform);
        } else
        {
            _carrying.transform.SetParent(null);
        }
    }

    public GameObject GetCarryables()
    {
        GameObject foundObj = null;
        var colliders = Physics.OverlapSphere(transform.position, _interactHitbox.radius);
        float closest = 999.0f;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == gameObject) continue;
            float dist = (transform.position - colliders[i].transform.position).magnitude;
            if (dist < closest)
            {
                if (colliders[i].TryGetComponent<MovementController>(out MovementController controller))
                {
                    closest = dist;
                    foundObj = colliders[i].gameObject;
                }
            }
        }
        return foundObj;
    }

    private void Carry(GameObject toCarry)
    {
        if (stack != null) return;
        if (IsCarrying) return;
        Debug.Log("FOUND: " + toCarry);
        if (toCarry.TryGetComponent<MovementController>(out MovementController controller))
        {
            Debug.Log("YUP");
            controller.enabled = false;
            _carrying = toCarry;
            ApplyCarry();
            return;
        }
    }
    private void Drop()
    {
        if (!IsCarrying) return;
        RevertCarry();
        if (_carrying.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            controller.enabled = false;

            // NOTE: CAST BEFORE PUTTING
            _carrying.transform.position = _carrying.transform.position + new Vector3(2 * (_movementController.isRight ? 1 : -1), 0, 0);

            controller.enabled = true;
        }
        _carrying = null;
    }
    private void Throw()
    {
        if (!IsCarrying) return;
        RevertCarry();
        if (_carrying.TryGetComponent<MovementController>(out MovementController controller))
        {
            controller.AddVelocity(new Vector3(2 * (_movementController.isRight ? 1 : -1),2,0));
        }
        _carrying = null;
    }

    public override void Controls(PlayerInput input)
    {
        base.Controls(input);
        if (input.actions["Special"].WasPressedThisFrame())
        {
            if (IsCarrying)
            {
                Drop();
            } else
            {
                var find = GetCarryables();
                Debug.Log(find);
                if (find != null)
                {
                    Carry(find);
                }
            }
        }
        if (input.actions["RemoveStack"].WasPressedThisFrame() && IsCarrying)
        {
            Throw();
        }
    }
}
