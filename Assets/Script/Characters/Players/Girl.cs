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
    private bool _controllingEnemy;

    private bool _enemyInFOV;
    private int _enemyTargetIndex;
    [SerializeField] private BaseEnemy _selectedEnemy;

    public delegate void Control(GameObject target);

    public event Control OnControlChange;
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

        if (Input.GetKeyDown(_controlEnemyKey) && _selectedEnemy)
	        ControlEnemy();

        if (_enemyInFOV && !_controllingEnemy)
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
        _animator.SetFloat("VelZ", 0);
        _animator.SetFloat("VelX", 0);
    }
    
    private void ControlEnemy()
    {
	    if (!_selectedEnemy) return;
	    
	    //If not controlling an enemy controls it.
	    if (_controllingEnemy == false)
	    {
		    _animator.SetTrigger("Poder");
		    _controllingEnemy = true;
		    _canMove = false;
		    OnControlChange?.Invoke(_selectedEnemy.gameObject);
		    
		    _selectedEnemy.Select();
		    
		    StartCoroutine(ControlTimer());
	    }
	    //If controlling an enemy, cancels the control.
	    else
	    {
		    _controllingEnemy = false;
		    _canMove = true;
		    OnControlChange?.Invoke(gameObject);
		    StopCoroutine(ControlTimer());
		    _selectedEnemy.Deselect();
		    StartCoroutine(ActivateSelectionIconWithTimer());
	    }
    }

    IEnumerator ControlTimer()
    {
	    yield return new WaitForSeconds(_controlMaxTime);
	    
	    ControlEnemy();
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
}
