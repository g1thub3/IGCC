using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField]
    float _damageVal = 1;
    [SerializeField] bool _killOnHit = true;
    [SerializeField] float _hitForce = 0.0f;
    Inventory _inventory;

    private void Start()
    {
        _inventory = FindAnyObjectByType<Inventory>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            if (_killOnHit)
            {
                health.takeDamage(_damageVal);
                var controller = collision.GetComponent<MovementController>();
                collision.GetComponent<MovementController>().AddVelocity((collision.transform.position - transform.position).normalized * _hitForce);
                AudioManager.Instance.PlaySFXOneShot("sfx_slaphurt", transform.position);
            }
            else
            {
                var control = collision.GetComponent<MovementController>();
                if (control != null && control.enabled && !health.isInvincible())
                {
                    health.takeDamage(0);
                    _inventory.changeLivesBy(-1);
                    var controller = collision.GetComponent<MovementController>();
                    collision.GetComponent<MovementController>().AddVelocity((collision.transform.position - transform.position).normalized * _hitForce);
                    AudioManager.Instance.PlaySFXOneShot("sfx_slaphurt", transform.position);
                }
            }
        }
    }
}
