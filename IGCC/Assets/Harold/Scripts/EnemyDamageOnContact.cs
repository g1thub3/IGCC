using Unity.VisualScripting;
using UnityEngine;

public class EnemyDamageOnContact : MonoBehaviour
{
    [SerializeField]
    float _damageVal = 1;

    [SerializeField]
    float _attackRadius=1;

    [SerializeField]
    LayerMask _layerOfAttack;

    [SerializeField]
    float _halfSize=0.25f;

    [SerializeField] Vector3 _boxDimensions = Vector3.one;

    [SerializeField] bool _isSphere = true;
    [SerializeField] bool _canHitWhite = false;
    [SerializeField] float _hitForce = 6.0f;
    [SerializeField] bool _killOnHit = true;

    Inventory _inventory;
    RespawnManager _respawn;
    private void Start()
    {
        _inventory = FindAnyObjectByType<Inventory>();
        _respawn = FindAnyObjectByType<RespawnManager>();
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRadius, _layerOfAttack);
        if (_isSphere)
        {
            hits = Physics.OverlapSphere(transform.position, _attackRadius, _layerOfAttack);
        } else
        {
            hits = Physics.OverlapBox(transform.position, _boxDimensions * 0.5f, transform.rotation, _layerOfAttack);
        }
        foreach (Collider hit in hits)
        {
            Collider collider = hit;

            if (collider == null) continue;

            //Try doing damage
            Health health = collider.GetComponent<Health>();
            MovementController controller = collider.GetComponent<MovementController>();
            bool whitePresent = (collider.GetComponent<WhiteMonkey>() != null);
            if ((whitePresent && !_canHitWhite) || (health && health.transform.position.y > transform.position.y + _halfSize) || !controller.enabled)
                return;

            //If not null deal damage to the entity
            if (health)
            {
                if (_killOnHit) {
                    //Do not damage if it's a white monkey
                    health.takeDamage(_damageVal);
                    collider.GetComponent<MovementController>().AddVelocity((collider.transform.position - transform.position).normalized * _hitForce);
                    AudioManager.Instance.PlaySFXOneShot("sfx_slaphurt", transform.position);
                } else
                {
                    if (!health.isInvincible())
                    {
                        health.takeDamage(0);
                        _inventory.changeLivesBy(-1);
                        AudioManager.Instance.PlaySFXOneShot("sfx_slaphurt", transform.position);
                        collider.GetComponent<MovementController>().AddVelocity((collider.transform.position - transform.position).normalized * _hitForce);
                        if (_inventory.Lives <= 0)
                            _respawn.respawn();
                    }
                }
                //Debug.Log("Entity took dmg");
            }

            //Debug.Log("attempt to hit");

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (_isSphere)
        {
            Gizmos.DrawWireSphere(transform.position, _attackRadius);
        } else
        {
            Gizmos.DrawWireCube(transform.position, _boxDimensions);
        }
    }
}
