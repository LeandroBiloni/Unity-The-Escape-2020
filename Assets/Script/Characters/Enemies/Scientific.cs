using System;
using System.Collections;
using UnityEngine;

public class Scientific : BaseEnemy
{
    [SerializeField] private KeyCode _talkKey;
    [SerializeField] private GameObject _textCloud;
    [SerializeField] private AlarmComputer _alarm;
    [SerializeField] private Transform _hideSpot; //Para que no active la alarma a cada rato
    private bool _isScared;
    private bool _isRunning;

    private Vector3 _playerPos;

    [SerializeField] private PatrolGuard _guardInFOV;


    protected override void Start()
    {
        base.Start();
        _canBeControlled = true;
        _textCloud.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_selected)
        { 
            _navMeshAgent.isStopped = true;
            CheckGuardInFOV();

            if (_guardInFOV && Input.GetKeyDown(_talkKey))
            {
                TalkToGuard();
            }
            return;
        }
        if (_isScared || _isTalking) return;
        
        if (!_isRunning) CheckWaypointDistance();

        if (!_selected && _playerInFOV && !_isRunning) GoToAlarm();
    }
    
    public override void Select()
    {
        base.Select();
        _animator.SetFloat("VelZ", 0);
        _animator.SetBool("MoveToAlarm", false);
        _animator.SetBool("Dizzy", false);
        _isRunning = false;
        _navMeshAgent.isStopped = true;
    }

    public override void Deselect()
    {
        base.Deselect();
        _navMeshAgent.isStopped = false;
        _canMove = true;
        _isScared = false;
    }

    private void GoToAlarm()
    {
        StopCoroutine(RotateTowards(Vector3.zero, true));

        _isRunning = true;
        _navMeshAgent.isStopped = true;
        _animator.SetBool("MoveToAlarm", true);
        var dir = Vector3.zero;
        if (_alarm)
        {
            var alarmPosition = _alarm.transform.position;
            _navMeshAgent.SetDestination(alarmPosition);
            _playerPos = _fieldOfView.visibleTargets[0].transform.position;
            dir = (alarmPosition - transform.position).normalized;
        }
        else
        {
            _navMeshAgent.SetDestination(_hideSpot.position);
            _isRunning = true;

            dir = (_hideSpot.position - transform.position).normalized;
            StartCoroutine(CheckDistanceToHide());
        }
        
        if (CheckIfNeedToRotate(dir))
        {
            StartCoroutine(RotateTowards(dir, true));
        }
        else _navMeshAgent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var alarm = other.gameObject.GetComponent<AlarmComputer>();
        if (alarm && !_selected && !_isScared)
        {
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

    private void CheckGuardInFOV()
    {
        if (_isTalking) return;
        if (_fieldOfView.visibleTargets.Count > 0)
        {
            
            if (_fieldOfView.visibleTargets[0].tag == "Patrol")
            {
                _guardInFOV = _fieldOfView.visibleTargets[0].GetComponent<PatrolGuard>();
                _guardInFOV.InScientificFOV();
            }
            else
            {
                if (_guardInFOV)
                {
                    _guardInFOV.OutOfScientificFOV();
                }
                _guardInFOV = null;   
            }
        }
        else
        {
            if (_guardInFOV)
            {
                _guardInFOV.OutOfScientificFOV();
            }
            _guardInFOV = null; 
        }
    }

    public void TalkToGuard()
    {
        FindObjectOfType<Girl>().CancelEnemyControl();
        _guardInFOV.StopToTalk();

        _navMeshAgent.SetDestination(_guardInFOV.transform.position);
        _canBeControlled = false;
        _isTalking = true;
        _fieldOfView.viewMeshFilter.gameObject.SetActive(false);
        _fieldOfView.enabled = false;
        
        StartCoroutine(TalkTimer());
        
        
    }
    
    IEnumerator TalkTimer()
    {
        while (_navMeshAgent.remainingDistance >= 1)
        {
            _animator.SetFloat("VelZ", _navMeshAgent.remainingDistance);
            yield return new WaitForEndOfFrame();
        }
        _animator.SetFloat("VelZ", 0);
        _textCloud.SetActive(true);
        Debug.Log("text active");
        Debug.Log(_textCloud.activeSelf);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.SetDestination(transform.position);
        _guardInFOV.StartTalk();

        yield return new WaitForSeconds(_talkTime);
        _canBeControlled = true;
        _isTalking = false;
        _navMeshAgent.isStopped = false;
        _fieldOfView.viewMeshFilter.gameObject.SetActive(true);
        _fieldOfView.enabled = true;
        _textCloud.SetActive(false);
        _guardInFOV.OutOfScientificFOV();
        _guardInFOV = null;
    }
}
