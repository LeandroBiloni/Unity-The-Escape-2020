﻿using System.Collections;
using UnityEngine;

public class PatrolGuard : BaseEnemy
{
    private bool _isChasing;
    [SerializeField] private GameObject _textCloud;
    
    protected override void Start()
    {
        base.Start();
        _textCloud.SetActive(false);
    }
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

        if (_canBeControlled)
            _canBeControlled = false;
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
            _animator.SetTrigger("Punch");
            other.gameObject.GetComponent<Character>().Dead();
            _navMeshAgent.isStopped = true;
        }
    }

    public void InScientificFOV()
    {
        if (_interactionIcon)
            _interactionIcon.SetActive(true);
    }

    public void OutOfScientificFOV()
    {
        if (_interactionIcon)
            _interactionIcon.SetActive(false);
    }

    public void StopToTalk()
    {
        _navMeshAgent.isStopped = true;
        _fieldOfView.viewMeshFilter.gameObject.SetActive(false);
        _fieldOfView.enabled = false;
        _isTalking = true;
        _animator.SetFloat("VelZ", 0);
    }

    public void StartTalk()
    {
        _textCloud.SetActive(true);
        StartCoroutine(TalkTimer());
    }

    IEnumerator TalkTimer()
    {
        yield return new WaitForSeconds(_talkTime);
        _textCloud.SetActive(false);
        _isTalking = false;
        _navMeshAgent.isStopped = false;
        _fieldOfView.viewMeshFilter.gameObject.SetActive(true);
        _fieldOfView.enabled = true;
    }
}
