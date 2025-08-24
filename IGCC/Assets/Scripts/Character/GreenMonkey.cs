using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GreenMonkey : Monkey
{
    private GameObject _carrying;
    private float _oldFOV;
    private float _newFOV = 90;
    private float _transTime = 0.25f;
    private Transition _fovTrans;
    private int _triggerCount;
    

    public bool IsCarrying {
        get { return _carrying != null;}
    }

    protected new void Start()
    {
        base.Start();
        _carrying = null;
        index = 1;
        _oldFOV = _charHandler.virtualCam.Lens.FieldOfView;
        _fovTrans = new Transition(_transTime);
        _triggerCount = 0;
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
        if (toCarry.TryGetComponent<MovementController>(out MovementController controller))
        {
            controller.enabled = false;
            _carrying = toCarry;
            ApplyCarry();
            return;
        }
    }
    private void Drop()
    {
        if (!IsCarrying) return;
        int dir = (_movementController.isRight ? 1 : -1);
        bool castCheck = Physics.Raycast(_carrying.transform.position, Vector2.right * dir, 2, LayerMask.GetMask("Ground"));
        if (castCheck) return;
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
            controller.AddVelocity(new Vector3(8 * (_movementController.isRight ? 1 : -1),0.3f,0));
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

    private IEnumerator FOVCoroutine(bool isIn)
    {
        _triggerCount++;
        int curr = _triggerCount;
        float diff = _newFOV - _oldFOV;
        _fovTrans.t = 0;
        var camData = Camera.main.GetUniversalAdditionalCameraData();

        while (_fovTrans.Progression < 1.0f)
        {
            if (_triggerCount != curr)
                break;
            _fovTrans.Progress();
            if (isIn)
            {
                _charHandler.virtualCam.Lens.FieldOfView = _oldFOV + (diff * _fovTrans.Progression);
            } else
            {
                _charHandler.virtualCam.Lens.FieldOfView = _newFOV - (diff * _fovTrans.Progression);
            }
            for (int i = 0; i < camData.cameraStack.Count; i++)
            {
                camData.cameraStack[i].fieldOfView = _charHandler.virtualCam.Lens.FieldOfView;
            }
            yield return new WaitForEndOfFrame();
        }
        if (_triggerCount == curr){
            _charHandler.virtualCam.Lens.FieldOfView = isIn ? _newFOV : _oldFOV;
            for (int i = 0; i < camData.cameraStack.Count; i++)
            {
                camData.cameraStack[i].fieldOfView = _charHandler.virtualCam.Lens.FieldOfView;
            }
        }
    }

    public override void OnDeSwitch()
    {
        base.OnSwitch();
        StartCoroutine(FOVCoroutine(false));
    }

    public override void OnSwitch()
    {
        base.OnSwitch();
        StartCoroutine(FOVCoroutine(true));
    }
}
