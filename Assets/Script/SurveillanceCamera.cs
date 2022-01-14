using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
	[SerializeField] private Transform _spawnPoint;

	[SerializeField] private Door _door;
	
	[SerializeField] private float _rotateAmount;
	[SerializeField] private bool _targetInFov;

	private int _targetIndex = 0;
	[SerializeField] private bool _moving;
	private FieldOfView _fieldOfView;
	Transform _target;

	Vector3 _startPosition;
	Quaternion _startRotation;

	private bool _alarmActivated;
	public delegate void AlarmActivation(Vector3 playerPos, Vector3 spawnPoint, Door door);

	public event AlarmActivation OnAlarmActivation;

	// Use this for initialization
	void Start()
	{
		_fieldOfView = GetComponentInChildren<FieldOfView>();
		_startPosition = transform.position;
		_startRotation = transform.rotation;
	}

	// Update is called once per frame
	void Update()
	{
		CheckFov();

		if (!_targetInFov && _moving)
		{
			Rotate();
		}
		else if (_targetInFov)
		{
			FollowTarget();
			
			if (!_alarmActivated)
				TriggerAlarm();
			return;
		}

		if (!_moving && !_targetInFov)
		{
			ResetPosition();
		}
	}

	private void Rotate()
	{
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, (Mathf.Sin(Time.realtimeSinceStartup) * _rotateAmount) + transform.eulerAngles.y, transform.eulerAngles.z);
	}

	private void FollowTarget()
	{
		_moving = false;
		transform.LookAt(_target);
	}

	private void ResetPosition()
	{
		_moving = true;
		transform.position = _startPosition;
		transform.rotation = _startRotation;
	}

	private void CheckFov()
	{
		if (_fieldOfView.visibleTargets.Count > 0)
		{
			if (_fieldOfView.visibleTargets[0].layer == LayerMask.NameToLayer("Player"))
			{
				_targetInFov = true;
				_target = _fieldOfView.visibleTargets[0].transform;
			}
		}
		else
		{
			_targetInFov = false;
			_target = null;
		}
	}

	private void TriggerAlarm()
	{
		var manager = FindObjectOfType<AlarmsManager>();
		
		if (manager.IsAlarmActive()) return;

		_alarmActivated = true;
		var playerPos = _fieldOfView.visibleTargets[0].transform.position;
		
		if (_door)
			_door.OpenDoor();

		OnAlarmActivation?.Invoke(playerPos, _spawnPoint.position, _door);
		manager.OnDeactivation += () => { _alarmActivated = false; };
	}
}
