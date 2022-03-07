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
	[SerializeField] private AudioClip _controlSfx;
	[SerializeField] private AudioClip _blindSfx;
	[SerializeField] private AudioClip _deathSfx;
	private bool _canControlEnemy = true;
	private bool _controllingEnemy;
	private bool _inCooldown;

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
        
        List<GameObject> list = new List<GameObject>();
        
        list.Add(_selectionIcon);

        StartCoroutine(IconsOffOnStart(list));
    }

    protected override void Update()
    {
        if (!_selected) return;
        
        base.Update();
        
        CheckEnemyInFOV();

        if (!_inCooldown && _selectedEnemy)
        {
	        if (Input.GetKeyDown(_controlEnemyKey))
	        {
	        
		        if (!_controllingEnemy)
			        ControlEnemy();
		        else if (_controllingEnemy)
			        CancelEnemyControl();
	        }

	        if (Input.GetKeyDown(_nextTargetKey) && !_controllingEnemy)
	        {
		        Blind();
	        }
        }
        
	        

        // if (_canControlEnemy && _enemyInFOV && !_controllingEnemy)
        // {
	       //  if (Input.GetKeyDown(_nextTargetKey))
		      //   NextTarget();
        //
	       //  if (Input.GetKeyDown(_previousTargetKey))
		      //   PreviousTarget(); 
        // }
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
	    
	    //If not controlling an enemy controls it.
	    if (_canControlEnemy)
	    {
			_audioManager.PlaySFX(_controlSfx, 1f);
		    _animator.SetTrigger("Poder");
		    _controllingEnemy = true;
		    _canControlEnemy = false;
		    _canMove = false;
		    OnControlChange?.Invoke(_selectedEnemy.gameObject);
		    
		    _selectedEnemy.Select();
		    StartTimer();
	    }
	    
    }

    public void CancelEnemyControl()
    {
	    if (!_controllingEnemy) return;
	    
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
	    StartCoroutine(AbilityCooldown());
    }

    IEnumerator ControlTimer()
    {
	    if (!_controllingEnemy) yield break;
	    
	    float time = _controlMaxTime;

	    while (time > 0)
	    {
		    if (_inCooldown) yield break;
		    time -= Time.deltaTime;

		    var value = time / _controlMaxTime;
		    OnTimerRunning?.Invoke(value);

		    yield return new WaitForEndOfFrame();
	    }
	    CancelEnemyControl();
    }

    IEnumerator AbilityCooldown()
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

    private void Blind()
    {
	    if (_inCooldown) return;

	    if (_selectedEnemy)
	    {
			_audioManager.PlaySFX(_blindSfx, 1f);
		    _selectedEnemy.Blind(_controlCooldown);
		    _animator.SetTrigger("Poder");
		    _canControlEnemy = false;
		    _inCooldown = true;
		    StartCooldown();
	    }
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
		if (watching && _canControlEnemy)
		{
			if (_selectedEnemy == null)
			{
				
				if (selectedEnemy && (selectedEnemy.CanBeControlled() || !selectedEnemy.IsBlinded() || !selectedEnemy.IsTalking()))
				{
					_selectedEnemy = selectedEnemy;
					selectedEnemy.UnitInPlayerFOV();
				}
			}

			if (_selectedEnemy && _selectedEnemy != selectedEnemy)
			{
				_selectedEnemy.UnitOutOfPlayerFOV();

				if (selectedEnemy.CanBeControlled())
				{
					_selectedEnemy = selectedEnemy;
				
					selectedEnemy.UnitInPlayerFOV();
				}
			}
		}
		else if (_selectedEnemy && !watching && !_controllingEnemy)
		{
			_selectedEnemy.UnitOutOfPlayerFOV();
			_selectedEnemy = null;
			_enemyTargetIndex = 0;
		}
	}
    
    // void NextTarget()
    // {
    //
	   //  _enemyTargetIndex--;
	   //  if (_enemyTargetIndex < 0)
	   //  {
		  //   _enemyTargetIndex = _fieldOfView.visibleTargets.Count-1;
	   //  }
	   //  var enemy = _fieldOfView.visibleTargets[_enemyTargetIndex].GetComponent<BaseEnemy>();
	   //  EnemyInPlayerFOV(_enemyInFOV, enemy);
	   //  
    // }
    //
    // void PreviousTarget()
    // {
    //
	   //  _enemyTargetIndex++;
	   //  if (_enemyTargetIndex == _fieldOfView.visibleTargets.Count)
	   //  {
		  //   _enemyTargetIndex = 0;
	   //  }
	   //  var enemy = _fieldOfView.visibleTargets[_enemyTargetIndex].GetComponent<BaseEnemy>();
	   //  EnemyInPlayerFOV(_enemyInFOV, enemy);
    // }

    public override void Dead()
    {
	    base.Dead();
	    Debug.Log("dead chica");
		_audioManager.PlaySFX(_deathSfx, 1f);
	    _canControlEnemy = false;
    }

    public bool IsControllingEnemy()
    {
	    return _controllingEnemy;
    }
}
