using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientific : BaseEnemy
{
    public Transform alarm;
    [SerializeField] private bool _isScared;
    private bool _isRunning;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (_isScared) return;
        
        if (!_isRunning) CheckWaypointDistance();

        if (_playerInFOV && !_isRunning) GoToAlarm();
    }
    
    public override void Select()
    {
        base.Select();
        _canMove = true;
        _navMeshAgent.isStopped = true;
        _animator.SetFloat("VelZ", 0);
        _animator.SetBool("MoveToAlarm", false);
        _animator.SetBool("Dizzy", false);
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    public override void Deselect()
    {
        base.Deselect();
        _navMeshAgent.isStopped = true;
        _canMove = false;
        _isScared = true;
        _animator.SetBool("Dizzy", true);
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    private void GoToAlarm()
    {
        StopCoroutine(RotateTowards(Vector3.zero));
        
        _isRunning = true;
        _navMeshAgent.isStopped = true;
        _animator.SetBool("MoveToAlarm", true);
        _navMeshAgent.SetDestination(alarm.position);
        
        var dir = (alarm.position - transform.position).normalized;
        
        if (CheckIfNeedToRotate(dir))
        {
            StartCoroutine(RotateTowards(dir));
        }
        else _navMeshAgent.isStopped = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Alarm"))
        {
            _navMeshAgent.isStopped = true;
            _isRunning = false;
            _isScared = true;
            _animator.SetBool("Dizzy", true);
            _animator.SetBool("MoveToAlarm", false);
        }
    }
}
