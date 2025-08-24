using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField]
    float _damageVal = 1;


    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
            health.takeDamage(_damageVal);
    }
}
