using UnityEngine;

public class StationaryEnemy : MonoBehaviour
{
    private Health _health;
    private void Start()
    {
        _health = GetComponent<Health>();
        _health.OnDeathEvent += OnDeath;
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
