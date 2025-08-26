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

    [SerializeField] bool isSphere = true;

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRadius, _layerOfAttack);
        if (isSphere)
        {
            hits = Physics.OverlapSphere(transform.position, _attackRadius, _layerOfAttack);
        } else
        {
            hits = Physics.OverlapBox(transform.position, _boxDimensions * 0.5f, transform.rotation, _layerOfAttack);
            Debug.Log(hits);
        }
        foreach (Collider hit in hits)
        {
            Collider collider = hit;

            if (collider == null) continue;

            //Try doing damage
            Health health = collider.GetComponent<Health>();

            WhiteMonkey white = collider.GetComponent<WhiteMonkey>();

            if (white || (health && health.transform.position.y > transform.position.y + _halfSize))
                return;

            //If not null deal damage to the entity
            if (health)
            {
                //Do not damage if it's a white monkey
                health.takeDamage(_damageVal);
                collider.GetComponent<MovementController>().AddVelocity((collider.transform.position - transform.position).normalized * 6.0f);
                //Debug.Log("Entity took dmg");
            }

            //Debug.Log("attempt to hit");

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (isSphere)
        {
            Gizmos.DrawWireSphere(transform.position, _attackRadius);
        } else
        {
            Gizmos.DrawWireCube(transform.position, _boxDimensions);
        }
    }
}
