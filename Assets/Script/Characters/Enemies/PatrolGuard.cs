using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGuard : BaseEnemy
{
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();

    [SerializeField] private int _waypointsIndex;

    private bool _playerInFOV;

    private bool _forward;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _forward = true;
        _navMeshAgent.SetDestination(_waypoints[_waypointsIndex].position);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
       // CheckPlayerInFOV();

        CheckWaypointDistance();
    }
    
    private void CheckPlayerInFOV()
    {
        if (_fieldOfView.visibleTargets.Count > 0)
        {
            if (_fieldOfView.visibleTargets[0].layer == LayerMask.NameToLayer("Player"))
                _playerInFOV = true;
        }  
        else _playerInFOV = false;
    }

    private void WaypointSelection()
    {
        if (_forward)
        {
            if (_waypointsIndex >= _waypoints.Count-1)
            {
                _forward = false;
                _waypointsIndex --;
            } 
            else _waypointsIndex++;
        }
        else
        {
            if (_waypointsIndex == 0)
            {
                _forward = true;
                _waypointsIndex++;
            }
            else _waypointsIndex--;
        }

        var target = _waypoints[_waypointsIndex].position;
        
        if (target != _navMeshAgent.destination)
            _navMeshAgent.SetDestination(target);
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(_navMeshAgent.destination, transform.position) <= _navMeshAgent.stoppingDistance)
        {
            WaypointSelection();
        }
    }
}
