using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformButton : MonoBehaviour
{
	[SerializeField] private GameObject _platform;
	[SerializeField] private bool _moveRight;
	[SerializeField] private bool _moveLeft;
	[SerializeField] private bool _moveForward;
	[SerializeField] private bool _moveBackward;
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
		
		if (_activated && _moveRight)
		{
			MoveRight();
		}
		else if(_activated && _moveLeft)
		{
			MoveLeft();
		}
		else if (_activated && _moveForward)
		{
			MoveForward();
		}
		else if (_activated && _moveBackward)
		{
			MoveBackward();
		}
	}

	void MoveRight()
	{
		Debug.Log("right");
		_platform.transform.position += _platform.transform.right * (_speed * Time.deltaTime);;
	}

	void MoveLeft()
	{
		Debug.Log("left");
		_platform.transform.position -= _platform.transform.right * (_speed * Time.deltaTime);;
	}

	void MoveForward()
	{
		Debug.Log("forward");
		_platform.transform.position += _platform.transform.forward * (_speed * Time.deltaTime);
	}

	void MoveBackward()
	{
		Debug.Log("backward");
		_platform.transform.position -= _platform.transform.forward * (_speed * Time.deltaTime);
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
