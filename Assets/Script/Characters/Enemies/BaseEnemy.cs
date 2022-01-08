using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : Character
{
    [SerializeField] protected List<Transform> _waypoints = new List<Transform>();

    [SerializeField] protected int _waypointsIndex;

    protected bool _playerInFOV;

    protected bool _forward;
    
    protected Color _originalColor;

    protected NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _originalColor = GetComponent<MeshRenderer>().material.color;
        
        _forward = true;
        
        if (_waypoints.Count > 0)
            _navMeshAgent.SetDestination(_waypoints[_waypointsIndex].position);
    }

    protected override void Update()
    {
        base.Update();
        
        CheckPlayerInFOV();
    }

    public void UnitInPlayerFOV()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void UnitOutOfPlayerFOV()
    {
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    public override void Select()
    {
        base.Select();
        _canMove = true;
        _navMeshAgent.isStopped = true;
        _animator.SetFloat("VelZ", 0);
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    public override void Deselect()
    {
        base.Deselect();
        _navMeshAgent.isStopped = false;
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }
    
    protected void CheckPlayerInFOV()
    {
        if (_fieldOfView.visibleTargets.Count > 0)
        {
            if (_fieldOfView.visibleTargets[0].layer == LayerMask.NameToLayer("Player"))
                _playerInFOV = true;
        }
        else
        {
            _playerInFOV = false;
        }
    }

    protected void WaypointSelection()
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

        var dir = (target - transform.position).normalized;
        
        
        if (target != _navMeshAgent.destination)
                _navMeshAgent.SetDestination(target);

        if (CheckIfNeedToRotate(dir))
        {
            StartCoroutine(RotateTowards(dir, false));
        }
    }
    
    protected void CheckWaypointDistance()
    {
        var distance = Vector3.Distance(_navMeshAgent.destination, transform.position);
        _animator.SetFloat("VelZ", distance);
        if (distance <= _navMeshAgent.stoppingDistance)
        {
            WaypointSelection();
        }
    }

    /// <summary>
    /// Checks if angle between forward and dir is >1.
    /// </summary>
    /// <param name="dir">The direction to check.</param>
    /// <returns></returns>
    protected bool CheckIfNeedToRotate(Vector3 dir)
    {
        var angle = Vector3.Angle(transform.forward, dir);
        return angle > 1;
    }

    /// <summary>
    /// Rotates smoothly towards direction.
    /// </summary>
    /// <param name="dir">The direction to rotate.</param>
    /// <param name="ignorePlayer">If false, rotation will be stopped when player is in fov.</param>
    /// <returns></returns>
    protected IEnumerator RotateTowards(Vector3 dir, bool ignorePlayer)
    {
        Quaternion rotTarget = Quaternion.LookRotation(dir);

        var angle = Vector3.Angle(transform.forward, dir);

        var time = 0f;
        while (angle > 1)
        {
            if (!ignorePlayer && _playerInFOV) break;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, time);
            time += Time.deltaTime;
            angle = Vector3.Angle(transform.forward, dir);

            yield return new WaitForEndOfFrame();
        }

        _navMeshAgent.isStopped = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        

        for (int i = 0; i < _waypoints.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_waypoints[i].position, 0.5f);

            Gizmos.color = Color.blue;
            if (i + 1 != _waypoints.Count)
            {
                Gizmos.DrawLine(_waypoints[i].position, _waypoints[i+1].position);
            }
            
        }
    }
}
