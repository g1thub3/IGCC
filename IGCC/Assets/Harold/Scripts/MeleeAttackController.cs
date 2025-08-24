using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    [SerializeField]
    float _attackValue;
    [SerializeField]
    float _attackRadius;
    [SerializeField]
    float _attackRange;
    //[SerializeField]
    //float _attackKnockback = 1;

    //Animation
    [SerializeField]
    string _isAttackingHash;

    //Point to attack at
    Vector3 _attackPoint;
    Vector3 _attackDir;
    SpriteAnimationController _spriteAnimController;


    [SerializeField]
    LayerMask _layerOfAttack;


    [Header("Sound Effects")]
    private AudioSource _sfxAudioSrc;
    [SerializeField] AudioClip _attackStartAudioClip;
    [SerializeField] AudioClip _attackHitAudioClip;

    public event System.Action OnAttackTriggeredEvent;
    public event System.Action OnAttackHitEvent;
    public event System.Action OnAttackFinishEvent;



    private void Awake()
    {
        _spriteAnimController = GetComponent<SpriteAnimationController>();
        _sfxAudioSrc = GetComponent<AudioSource>();
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(_attackPoint,_attackRadius);
    //}

    public void doAttack()
    {
        if (_spriteAnimController.getBool(_isAttackingHash))
        {
            return;
        }

        //Play if not already attacking
        if (_attackStartAudioClip && _sfxAudioSrc && !_spriteAnimController.getBool(_isAttackingHash))
            _sfxAudioSrc.PlayOneShot(_attackStartAudioClip);
        //Start the attack animation
        _spriteAnimController.setBool(_isAttackingHash, true);

        OnAttackTriggeredEvent?.Invoke();
    }

    //Damage something at a point that was set by set Attack Direction 
    //Use animator to call this for animation sync
    public void attackHit()
    {

        if (_spriteAnimController.getBool(_isAttackingHash))
        {
            //Damage at point
            //_damager.DamageAtPoint(_attackValue, _attackRadius, attackPosition, _layerOfAttack);
            executeSequence();
            _spriteAnimController.setBool(_isAttackingHash, false);


            if (_attackHitAudioClip && _sfxAudioSrc)
                _sfxAudioSrc.PlayOneShot(_attackHitAudioClip);
            OnAttackHitEvent?.Invoke();

            OnAttackFinishEvent?.Invoke();
            //Debug.Log("Explode");
        }
    }

    public void finishAttack()
    {
        _spriteAnimController.setBool(_isAttackingHash, false);
        OnAttackFinishEvent?.Invoke();
        //Debug.Log("Explode");
    }

    private void OnDrawGizmosSelected()
    {
        // Normalize attack direction
        Vector3 dir = _attackDir.normalized;

        // Start & end positions of the sweep
        Vector3 start = transform.position;
        Vector3 end = start + dir * _attackRange;

        // Set gizmo color
        Gizmos.color = Color.red;

        // Draw start & end spheres
        Gizmos.DrawWireSphere(start, _attackRadius);
        Gizmos.DrawWireSphere(end, _attackRadius);

        // Draw capsule-like connection
        Gizmos.DrawLine(start + Vector3.up * _attackRadius, end + Vector3.up * _attackRadius);
        Gizmos.DrawLine(start - Vector3.up * _attackRadius, end - Vector3.up * _attackRadius);
        Gizmos.DrawLine(start + Vector3.right * _attackRadius, end + Vector3.right * _attackRadius);
        Gizmos.DrawLine(start - Vector3.right * _attackRadius, end - Vector3.right * _attackRadius);
    }

    public void executeSequence()
    {

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _attackRadius, _attackDir, _attackRange, _layerOfAttack);

        foreach (RaycastHit hit in hits)
        {
            Collider collider = hit.collider;

            if (collider == null) continue;

            //Try doing damage
            Health health = collider.GetComponent<Health>();

            //If not null deal damage to the entity
            if (health)
            {
                health.takeDamage(1);
                //Debug.Log("Entity took dmg");
            }

            //Debug.Log("attempt to hit");

        }
    }

    public void attackCancel()
    {
        _spriteAnimController.setBool(_isAttackingHash, false);
    }

    public void setAttackDirection(Vector3 attackDir)
    {
        _attackDir = attackDir.normalized;

        //Apply attackRange to it
        attackDir = transform.position + _attackRange * (attackDir);
        _attackPoint = attackDir;
    }

    public bool inAttackRange(Transform other)
    {
        if (transform == null)
            return false;

        return Vector3.Distance(transform.position, other.position) <= _attackRange;
    }
}
