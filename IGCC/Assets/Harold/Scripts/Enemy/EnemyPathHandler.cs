using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathHandler : MonoBehaviour
{
    //Patb the AI takes
    AIPath _aiPath;
    public AIPath AIPath => _aiPath;

    //Destination setter of AI
    AIDestinationSetter _destinationSetter;

    // Seeker component
    private Seeker _seeker;
    Path _currentPath;

    //Enemy Sight ref
    // EnemySight _enemySight;



    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _aiPath = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();

        //  _enemySight=GetComponent<EnemySight>();
    }

    //private void Start()
    //{
    //    _destinationSetter.target = _enemySight.PlayerRef;
    //}

    private void Start()
    {
        _seeker.pathCallback += OnPathComplete;
        // _destinationSetter.target = _enemySight.PlayerRef;
    }

    //Set whether we should continuously move with path or not
    public void setPathEnabled(bool state)
    {
        _aiPath.enabled = state;
    }

    //Set the AI target of the destination
    public void setTarget(Transform target)
    {
        _destinationSetter.target = target;
    }

    //Set movement Speed of path
    public void setSpeed(float speed)
    {
        _aiPath.maxSpeed = speed;
    }


    //Update when path complete
    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            _currentPath = path;
        }
    }

    //Get direction
    public Vector3 getDir()
    {
        if (_currentPath == null || _currentPath.vectorPath.Count<=0)
            return Vector3.zero;

        return transform.position - _currentPath.vectorPath[0];
    }

    //Get the current destination
    public Vector3 getDestination()
    {
        if (_currentPath == null)
            return Vector3.zero;

        return _currentPath.vectorPath[0];
    }

}
