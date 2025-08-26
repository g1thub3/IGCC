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


    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRadius, _layerOfAttack);

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
            if (health){
                //Do not damage if it's a white monkey
                health.takeDamage(_damageVal);
                //Debug.Log("Entity took dmg");
            }

            //Debug.Log("attempt to hit");

        }
    }
}
