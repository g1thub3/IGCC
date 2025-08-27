using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField]
    float _damageVal = 1;
    [SerializeField] bool _killOnHit = true;
    [SerializeField] float _hitForce = 0.0f;

    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null && _killOnHit)
        { 
            health.takeDamage(_damageVal);
            var controller = collision.GetComponent<MovementController>();
            collision.GetComponent<MovementController>().AddVelocity((collision.transform.position - transform.position).normalized * _hitForce);
        }
    }
}
