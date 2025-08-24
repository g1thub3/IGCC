using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A Basic los mechanic for an enemy
public class EnemySight : MonoBehaviour
{
    [SerializeField]
    private CharacterHandler _player;
    [SerializeField]
    private float _sightRange = 0f;
    public float SightRange => _sightRange;

    [SerializeField]
    private float _blindRange = 0f;
    [SerializeField]
    LayerMask _groundLayer;

    //The transform of the player this enemy targets
    public Transform Target
    {
        get
        {
            if (!_player || !_player.CurrMonkey)
                return null;

            return _player.CurrMonkey.transform;
        }
    }

    public void setTarget(CharacterHandler target)
    {
        _player = target;
    }


    public bool canSee()
    {
        if (_player == null || !_player.CurrMonkey)
            return false;
        float distanceToPlayer = Vector3.Distance(Target.position, transform.position);

        if (distanceToPlayer <= _blindRange)
            return false;

        //Check if player is within sight range
        Physics.Raycast(transform.position, (Target.position - transform.position).normalized, out RaycastHit hit, distanceToPlayer, _groundLayer);

        //If it hit ground then it cant see
        if (hit.collider != null)
        {
            return false;
        }


        return distanceToPlayer <= _sightRange;
    }

    public bool inRange(float range)
    {
        if (_player == null || !_player.CurrMonkey)
            return false;

        float distanceToPlayer = Vector3.Distance(Target.position, transform.position);

        if (distanceToPlayer <= _blindRange)
            return false;

        //Check if player is within sight range
        Physics.Raycast(transform.position, (Target.position - transform.position).normalized, out RaycastHit hit, distanceToPlayer, _groundLayer);

        //If it hit ground then it cant see
        if (hit.collider != null)
        {
            return false;
        }


        return distanceToPlayer <= range;
    }
}
