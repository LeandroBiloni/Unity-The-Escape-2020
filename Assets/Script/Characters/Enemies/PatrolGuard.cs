using System.Collections;
using UnityEngine;

public class PatrolGuard : BaseEnemy
{
    private bool _isChasing;
    [SerializeField] private KeyCode _talkKey;
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

    protected virtual void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Character>().Dead();
            _navMeshAgent.isStopped = true;
            _animator.SetTrigger("Punch");
            
            //TODO: derrota
        }
    }

    public void InScientificFOV()
    {
        _interactionIcon.SetActive(true);
    }

    public void OutOfScientificFOV()
    {
        _interactionIcon.SetActive(false);
    }

    public void StopToTalk()
    {
        _navMeshAgent.isStopped = true;
        _fieldOfView.viewMeshFilter.gameObject.SetActive(false);
        _fieldOfView.enabled = false;
        _canBeControlled = false;
        _isTalking = true;
        _animator.SetFloat("VelZ", 0);
    }

    public void StartTalk()
    {
        StartCoroutine(TalkTimer());
    }

    IEnumerator TalkTimer()
    {
        yield return new WaitForSeconds(_talkTime);
        _canBeControlled = true;
        _isTalking = false;
        _navMeshAgent.isStopped = false;
        _fieldOfView.viewMeshFilter.gameObject.SetActive(true);
        _fieldOfView.enabled = true;
    }
}
