using System;
using System.Collections;
using UnityEngine;

public class Scientific : BaseEnemy
{
    [SerializeField] private AlarmComputer _alarm;
    [SerializeField] private Transform _hideSpot; //Para que no active la alarma a cada rato
    private bool _isScared;
    private bool _isRunning;

    private Vector3 _playerPos;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (_isScared || _selected) return;
        
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
        //_navMeshAgent.isStopped = true;
        _navMeshAgent.isStopped = false;
        // _canMove = false;
        // _isScared = true;
        _canMove = true;
        _isScared = false;
        //_animator.SetBool("Dizzy", true);
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    private void GoToAlarm()
    {
        StopCoroutine(RotateTowards(Vector3.zero, true));

        if (_alarm)
        {
            _isRunning = true;
            _navMeshAgent.isStopped = true;
            _animator.SetBool("MoveToAlarm", true);
            var alarmPosition = _alarm.transform.position;
            _navMeshAgent.SetDestination(alarmPosition);
            _playerPos = _fieldOfView.visibleTargets[0].transform.position;
            var dir = (alarmPosition - transform.position).normalized;
        
            if (CheckIfNeedToRotate(dir))
            {
                StartCoroutine(RotateTowards(dir, true));
            }
            else _navMeshAgent.isStopped = false;
        }
        else
        {
            _navMeshAgent.SetDestination(_hideSpot.position);
            _isRunning = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var alarm = other.gameObject.GetComponent<AlarmComputer>();
        if (alarm && !_isScared)
        {
            Debug.Log("alarm");
            
            alarm.TriggerAlarm(_playerPos);

            _navMeshAgent.SetDestination(_hideSpot.position);
            StartCoroutine(CheckDistanceToHide());
        }
    }

    IEnumerator CheckDistanceToHide()
    {
        while (_navMeshAgent.remainingDistance >= 1)
        {
            yield return new WaitForEndOfFrame();
        }
        
        _navMeshAgent.isStopped = true;
        _isRunning = false;
        _isScared = true;
        _animator.SetBool("Dizzy", true);
        _animator.SetBool("MoveToAlarm", false);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (_hideSpot)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_hideSpot.position, Vector3.one); 
        }
    }
}
