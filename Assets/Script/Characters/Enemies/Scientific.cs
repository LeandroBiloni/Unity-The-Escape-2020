using UnityEngine;

public class Scientific : BaseEnemy
{
    public Transform alarm;
    [SerializeField] private bool _isScared;
    private bool _isRunning;

    private Vector3 _playerPos;
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
        StopCoroutine(RotateTowards(Vector3.zero, true));
        
        _isRunning = true;
        _navMeshAgent.isStopped = true;
        _animator.SetBool("MoveToAlarm", true);
        _navMeshAgent.SetDestination(alarm.position);
        _playerPos = _fieldOfView.visibleTargets[0].transform.position;
        var dir = (alarm.position - transform.position).normalized;
        
        if (CheckIfNeedToRotate(dir))
        {
            StartCoroutine(RotateTowards(dir, true));
        }
        else _navMeshAgent.isStopped = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        var alarm = other.gameObject.GetComponent<Alarm>();
        if (alarm && !_isScared)
        {
            Debug.Log("alarm");
            _navMeshAgent.isStopped = true;
            _isRunning = false;
            _isScared = true;
            _animator.SetBool("Dizzy", true);
            _animator.SetBool("MoveToAlarm", false);
            alarm.TriggerAlarm(_playerPos);
        }
    }
}
