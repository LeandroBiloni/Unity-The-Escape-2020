using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : Character
{
	[Header("Keys")]
	[SerializeField] private KeyCode _nextTargetKey;
	[SerializeField] private KeyCode _previousTargetKey;
	[SerializeField] private KeyCode _controlEnemyKey;
	[SerializeField] private float _controlMaxTime;
	[SerializeField] private float _controlCooldown;
	private bool _canControlEnemy = true;
	private bool _controllingEnemy;
	[SerializeField] private bool _inCooldown;

    private bool _enemyInFOV;
    private int _enemyTargetIndex;
    private BaseEnemy _selectedEnemy;

    public delegate void Control(GameObject target);
    public event Control OnControlChange;

    public delegate void Timer(float value);
    public event Timer OnTimerRunning;
    protected override void Start()
    {
        base.Start();
        var selector = FindObjectOfType<CharacterSelector>();

        selector.OnGirlSelect += Select;
        selector.OnBoySelect += Deselect;
    }

    protected override void Update()
    {
        if (!_selected) return;
        
        base.Update();
        
        CheckEnemyInFOV();

        if (!_inCooldown && Input.GetKeyDown(_controlEnemyKey))
        {
	        if (_selectedEnemy && !_controllingEnemy)
		        ControlEnemy();

	        else if (_controllingEnemy)
		        CancelEnemyControl();
        }
	        

        if (_canControlEnemy && _enemyInFOV && !_controllingEnemy)
        {
	        if (Input.GetKeyDown(_nextTargetKey))
		        NextTarget();
        
	        if (Input.GetKeyDown(_previousTargetKey))
		        PreviousTarget(); 
        }
    }
    
    public override void Select()
    {
        base.Select();
    }
    
    public override void Deselect()
    {
        base.Deselect();
        if (_animator)
        {
	        _animator.SetFloat("VelZ", 0);
	        _animator.SetFloat("VelX", 0);
        }
    }
    
    private void ControlEnemy()
    {
	    if (_controllingEnemy || _inCooldown) return;
	    
	    Debug.Log("control enemy");
	    //If not controlling an enemy controls it.
	    if (_canControlEnemy)
	    {
		    _animator.SetTrigger("Poder");
		    _controllingEnemy = true;
		    _canControlEnemy = false;
		    _canMove = false;
		    OnControlChange?.Invoke(_selectedEnemy.gameObject);
		    
		    _selectedEnemy.Select();
		    StartTimer();
	    }
	    
    }

    private void CancelEnemyControl()
    {
	    if (!_controllingEnemy) return;
	    
	    Debug.Log("control enemy cancel");
	    //If controlling an enemy, cancels the control.
	    _canMove = true;
	    _controllingEnemy = false;
	    OnControlChange?.Invoke(gameObject);
	    _inCooldown = true;
	    StopTimer();
	    OnTimerRunning?.Invoke(0);
	    
	    if (_selectedEnemy) _selectedEnemy.Deselect();
	    
	    StartCooldown();
	    StartCoroutine(ActivateSelectionIconWithTimer());
    }

    void StopTimer()
    {
	    StopCoroutine(ControlTimer());
    }

    void StartTimer()
    {
	    StartCoroutine(ControlTimer());
    }

    void StartCooldown()
    {
	    StartCoroutine(ControlCooldown());
    }

    IEnumerator ControlTimer()
    {
	    Debug.Log("control timer");
	    
	    if (!_controllingEnemy) yield break;
	    
	    float time = _controlMaxTime;

	    while (time > 0)
	    {
		    if (_inCooldown) yield break;
		    Debug.Log("control timer --");
		    time -= Time.deltaTime;

		    var value = time / _controlMaxTime;
		    OnTimerRunning?.Invoke(value);

		    yield return new WaitForEndOfFrame();
	    }
	    CancelEnemyControl();
    }

    IEnumerator ControlCooldown()
    {
	    
	    float time = 0;

	    while (time < _controlCooldown)
	    {
		    time += Time.deltaTime;

		    var value = time / _controlCooldown;
		    
		    OnTimerRunning?.Invoke(value);

		    yield return new WaitForEndOfFrame();
	    }

	    _inCooldown = false;
	    _canControlEnemy = true;
	    OnTimerRunning?.Invoke(1);
	    
    }

    void CheckEnemyInFOV()
    {
        if (_fieldOfView.visibleTargets.Count == 0) //Checkea si hay enemigos a la vista
        {
            _enemyInFOV = false;
            _enemyTargetIndex = 0;
            EnemyInPlayerFOV(_enemyInFOV, null);
        }
        else
        {
            _enemyInFOV = true;
            if (_fieldOfView.visibleTargets.Count == 1)
                _enemyTargetIndex = 0;
            var enemy = _fieldOfView.visibleTargets[_enemyTargetIndex];
            EnemyInPlayerFOV(_enemyInFOV, enemy.GetComponent<BaseEnemy>());
        }
    }
    
    private void EnemyInPlayerFOV(bool watching, BaseEnemy selectedEnemy)
	{
		if (watching)
		{
			if (_selectedEnemy == null)
			{
				_selectedEnemy = selectedEnemy;
				selectedEnemy.UnitInPlayerFOV();
			}

			if (_selectedEnemy != selectedEnemy)
			{
				_selectedEnemy.UnitOutOfPlayerFOV();

				_selectedEnemy = selectedEnemy;
				
				selectedEnemy.UnitInPlayerFOV();
			}
		}
		else if (_selectedEnemy && !watching && !_controllingEnemy)
		{
			_selectedEnemy.UnitOutOfPlayerFOV();
			_selectedEnemy = null;
			_enemyTargetIndex = 0;
		}
	}
    
    void NextTarget()
    {

	    _enemyTargetIndex--;
	    if (_enemyTargetIndex < 0)
	    {
		    _enemyTargetIndex = _fieldOfView.visibleTargets.Count-1;
	    }
	    var enemy = _fieldOfView.visibleTargets[_enemyTargetIndex].GetComponent<BaseEnemy>();
	    EnemyInPlayerFOV(_enemyInFOV, enemy);
	    
    }
    
    void PreviousTarget()
    {

	    _enemyTargetIndex++;
	    if (_enemyTargetIndex == _fieldOfView.visibleTargets.Count)
	    {
		    _enemyTargetIndex = 0;
	    }
	    var enemy = _fieldOfView.visibleTargets[_enemyTargetIndex].GetComponent<BaseEnemy>();
	    EnemyInPlayerFOV(_enemyInFOV, enemy);
    }

    public override void Dead()
    {
	    base.Dead();
	    _canControlEnemy = false;
    }
}
