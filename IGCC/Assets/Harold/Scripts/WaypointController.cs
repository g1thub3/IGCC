using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _waypoints;

    //Cur index that the object is moving towards
    [SerializeField]
    private int _curWayPoint;

    [SerializeField]
    private int _waypointDir = 1;

    [SerializeField]
    private float _speed = 30f;

    [SerializeField]
    private float _waypointNearValue = 0.5f;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void setSpeed(float speed)
    {
        _speed = speed;
    }


    private void FixedUpdate()
    {

        if (_rb == null || _waypoints.Count == 0)
            return;

        if (_curWayPoint > _waypoints.Count - 1)
        {
            _waypointDir = -1;
        }
        if (_curWayPoint < 0)
        {
            _waypointDir = 1;
        }

        _curWayPoint = Mathf.Clamp(_curWayPoint, 0, _waypoints.Count - 1);



        //Move towards cur waypoint
        _rb.MovePosition(transform.position + (_waypoints[_curWayPoint].position - transform.position).normalized * _speed * Time.deltaTime);

        //Rotate towards curwaypoint
        //_rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_waypoints[_curWayPoint].position - transform.position).normalized, _speed * Time.deltaTime));

        //Set the direction to opposing side if they reach end or start

        //
        if (Vector3.Magnitude(_waypoints[_curWayPoint].position - transform.position) <= _waypointNearValue)
        {
            _curWayPoint += _waypointDir;
        }

        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, Vector3.zero, Time.deltaTime);



    }


}