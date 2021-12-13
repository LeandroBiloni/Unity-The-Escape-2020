using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
	public float rotate_amount;
	public bool targetInFov;

	int _targetIndex = 0;
	bool _gotTargetPosicion;
	bool _callSecurity = true;
	bool _following;
	bool _moving;
	public GameManager _manager;
	public GuardSpawn spawn;
	public Door door;
	FieldOfView _fov;
	Transform _target;

	Vector3 startPosition;
	Quaternion startRotation;

	// Use this for initialization
	void Start()
	{
		_manager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
		_fov = GetComponentInChildren<FieldOfView>();
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

	// Update is called once per frame
	void Update()
	{
		CheckFov();

		if (!targetInFov)
		{
			_following = false;
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, (Mathf.Sin(Time.realtimeSinceStartup) * rotate_amount) + transform.eulerAngles.y, transform.eulerAngles.z);
		}
		else
		{
			_following = true;
			FollowTarget();
			//if(!_gotTargetPosicion)
				GetPlayerPosition(); 
		}

		if (!_moving && !targetInFov)
		{
			ResetPosition();
		}


		if (_callSecurity == false)
			CheckIfAlarmOff();
	}

	void FollowTarget()
	{
		_moving = false;
		_target = _fov.visibleTargets[_targetIndex].transform;
		transform.LookAt(_target);
	}

	void ResetPosition()
	{
		_following = false;
		_moving = true;
		transform.position = startPosition;
		transform.rotation = startRotation;
	}

	void CheckFov()
	{
		if (_fov.visibleTargets.Count > 0)
		{
			if (_fov.visibleTargets[0].layer == LayerMask.NameToLayer("Player"))
			{
				targetInFov = true;

				if (_callSecurity)
					CallSecurity();
			}
		}
		else
		{
			targetInFov = false;
			_gotTargetPosicion = false;
		}
	}

	void CallSecurity()
	{
		_callSecurity = false;
		_target = _fov.visibleTargets[_targetIndex].transform;
		_manager.ActivateAlarm(_target.position, true, spawn, door, _fov.visibleTargets[_targetIndex].tag);
	}

	private void GetPlayerPosition()
	{
		_gotTargetPosicion = true;
		print("GetPlayerPosition");
		_manager.TellPlayerPosition(_fov.visibleTargets[0].transform.position, _fov.visibleTargets[0].tag);
	}

	private void CheckIfAlarmOff()
	{
		if (_manager.alarmOn == false)
			_callSecurity = true;
	}
}
