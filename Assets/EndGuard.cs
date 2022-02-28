using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndGuard : BaseEnemy
{
    [SerializeField] private GameObject _textCloud;
    // Start is called before the first frame update
    protected override void Start()
    {
        _fieldOfView = GetComponent<FieldOfView>();
        _selectionIcon.SetActive(false);
        _particle.gameObject.SetActive(false);
        _animator = GetComponent<Animator>();
        FieldOfViewOff();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.isStopped) return;
        
        CheckStop();
    }

    public void Activate()
    {
        _animator.SetBool("Chasing", true);
        _navMeshAgent.SetDestination(_waypoints[0].position);
        _navMeshAgent.isStopped = false;
    }

    private void CheckStop()
    {
        if (_waypoints.Count <= 0) return;

        var distance = Vector3.Distance(_navMeshAgent.destination, transform.position);
        _animator.SetFloat("VelZ", distance);
        if (distance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.isStopped = true;
            _animator.SetFloat("VelZ", 0);
            _animator.SetBool("Chasing", false);
            _textCloud.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            _textCloud.SetActive(true);
        }
    }
}
