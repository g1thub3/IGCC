using JetBrains.Annotations;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    MovementController _movementController;

    [SerializeField] float _damageVal=1;
    [SerializeField] float _groundRay = 1.25f;
    [SerializeField] float _castRange = 0.8f;
    [SerializeField] LayerMask _enemyLayer;

    public void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private bool doAttack()
    {
        Vector3 charOrigin = transform.position - new Vector3(0, _movementController.Controller.height * 0.35f, 0);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 currOrigin = charOrigin + new Vector3(_movementController.Controller.radius * i * _castRange, 0, _movementController.Controller.radius * j * _castRange);
                bool isGrounded = Physics.Raycast(currOrigin, -transform.up,out RaycastHit hit, _groundRay, _enemyLayer);

                if (isGrounded)
                {
                    Health health = hit.collider.GetComponent<Health>();
                    health.takeDamage(_damageVal);
                    return true;
                }
            }
        }
        //bool isGrounded = Physics.CapsuleCast(origin, origin + new Vector3(0, -_groundRay, 0), 
        //    _controller.radius - 0.1f, Vector3.down, _groundRay, LayerMask.NameToLayer("Ground"));
        //bool isGrounded = Physics.Raycast(origin, -transform.up, _groundRay, LayerMask.GetMask("Ground"));
        //return isGrounded;
        return false;
    }


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        doAttack();
    }
}
