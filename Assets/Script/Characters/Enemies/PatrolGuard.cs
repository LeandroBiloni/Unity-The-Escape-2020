using UnityEngine;

public class PatrolGuard : BaseEnemy
{
    private bool _isChasing;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_navMeshAgent.isStopped) return;
        if (!_playerInFOV)
        {
            if (_isChasing)
            {
                _isChasing = false;
                _animator.SetBool("Chasing", false);
            }
            CheckWaypointDistance(); 
        }
        else PlayerChase();
    }
    
    private void PlayerChase()
    {
        _isChasing = true;
        _animator.SetBool("Chasing", true);
        var playerPos = _fieldOfView.visibleTargets[0].transform.position;
        _navMeshAgent.SetDestination(playerPos);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Character>().Dead();
            _animator.SetTrigger("Punch");
            
            //TODO: derrota
        }
    }
}
