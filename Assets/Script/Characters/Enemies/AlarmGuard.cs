using UnityEngine;
using UnityEngine.AI;

public class AlarmGuard : BaseEnemy
{
    private bool _isChasing;
    private bool _returnToSpawn;
    private Vector3 _spawnPosition;

    private bool _return;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (_navMeshAgent.isStopped) return;
        
        if (!_playerInFOV || _return)
        {
            if (_isChasing)
            {
                _isChasing = false;
                _animator.SetBool("Chasing", false);
            }
        }
        if (_playerInFOV && !_return)
             PlayerChase();
    }
    
    private void PlayerChase()
    {
        Debug.Log("chase");
        if (!_isChasing)
        {
            _isChasing = true;
            _animator.SetBool("Chasing", true);
        }
        
        var pos = _fieldOfView.visibleTargets[0].transform.position;
        _navMeshAgent.SetDestination(pos);
    }

    public void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
    }

    public void SetPlayerSeenPosition(Vector3 position)
    {
        if (!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(position);
        
        if (!_animator)
            _animator = GetComponent<Animator>();
        _animator.SetBool("Chasing", true);
    }

    
    public void ReturnToSpawn()
    {
        _isChasing = false;
        _return = true;
        var dir = (_spawnPosition - transform.position).normalized;
        if (CheckIfNeedToRotate(dir))
        {
            StartCoroutine(RotateTowards(dir, true));
        }
        _navMeshAgent.SetDestination(_spawnPosition);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Character>().Dead();
            _navMeshAgent.isStopped = true;
            _animator.SetTrigger("Punch");
            
            //TODO: derrota
        }
    }
}
