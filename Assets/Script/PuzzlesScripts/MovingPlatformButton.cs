using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformButton : MonoBehaviour
{
	[SerializeField] private GameObject _platform;
	[SerializeField] private bool _moveForward;
	[SerializeField] private bool _moveRight;
	private bool _forward;
	[SerializeField] private float _speed = 1f;
	private bool _activated;

	private MeshRenderer _meshRenderer;
	private void Start()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		if (!_meshRenderer.isVisible) return; 
		
		if (_activated && !_moveForward)
		{
			MoveRight();
		}
		else if (_activated && _moveForward)
		{
			MoveForward();
		}
	}

	void MoveRight()
	{
		var right = Vector3.right * (_speed * Time.deltaTime);
		if (_moveRight)
			_platform.transform.position += right;
		else
			_platform.transform.position -= right;
	}

	void MoveForward()
	{
		var forward = Vector3.forward * (_speed * Time.deltaTime);
		if (_forward)
			_platform.transform.position += forward;
		else
			_platform.transform.position -= forward;
	}
	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			_activated = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			_activated = false;
	}
}
