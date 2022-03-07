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
	[SerializeField] float _rotationSpeed = 1.5f;
	[SerializeField] Vector3 _targetRotation;
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
		_startRotation = transform.localRotation;
		StartCoroutine(CameraRotation(Quaternion.Euler(_targetRotation)));
	}

	// Update is called once per frame
	void Update()
	{
		CheckFov();

		if (_targetInFov)
		{
			StopAllCoroutines();
			//FollowTarget();
			_moving = false;
			if (!_alarmActivated)
				TriggerAlarm();
			return;
		}

		if (!_moving && !_targetInFov)
		{
			ResetPosition();
			StartCoroutine(CameraRotation(Quaternion.Euler(_targetRotation)));
		}
	}

	IEnumerator CameraRotation(Quaternion endValue)
	{
		float time = 0;
		var startValue = transform.localRotation;
		
		while (time < _rotationSpeed)
		{
			transform.localRotation = Quaternion.Lerp(startValue, endValue, time / _rotationSpeed);
			//transform.rotation.x = 0;
			time += Time.deltaTime;
			yield return null;
		}

		transform.localRotation = endValue;
		StartCoroutine(CameraRotateBack());
	}

	IEnumerator CameraRotateBack()
	{
		float time = 0;
		var startValue = transform.localRotation;

		while (time < _rotationSpeed)
		{
			transform.localRotation = Quaternion.Lerp(startValue, _startRotation, time / _rotationSpeed);
			time += Time.deltaTime;
			yield return null;
		}

		transform.localRotation = _startRotation;
		_moving = false;

		StopCoroutine(CameraRotateBack());
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
		transform.localRotation = _startRotation;
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
