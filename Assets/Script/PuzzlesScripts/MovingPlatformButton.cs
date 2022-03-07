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
	[SerializeField] private List<Cable> _cables = new List<Cable>();
	[SerializeField] private AudioManager _audioManager;
	[SerializeField] private AudioClip _activationSound;
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
		_platform.transform.position += _platform.transform.right * (_speed * Time.deltaTime);;
	}

	void MoveLeft()
	{
		_platform.transform.position -= _platform.transform.right * (_speed * Time.deltaTime);;
	}

	void MoveForward()
	{
		_platform.transform.position += _platform.transform.forward * (_speed * Time.deltaTime);
	}

	void MoveBackward()
	{
		_platform.transform.position -= _platform.transform.forward * (_speed * Time.deltaTime);
	}
	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer ==
		                                                              LayerMask.NameToLayer("Enemy")
		                                                              || other.gameObject.layer ==
		                                                              LayerMask.NameToLayer("MovableObjects"))
		{
			if(_audioManager)
				_audioManager.PlaySFX(_activationSound);

			_activated = true;
			CablesOn();
		}
			
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer ==
		                                                              LayerMask.NameToLayer("Enemy")
		                                                              || other.gameObject.layer ==
		                                                              LayerMask.NameToLayer("MovableObjects"))
		{
			if(_audioManager)
				_audioManager.PlaySFX(_activationSound);

			_activated = false;
			CablesOff();
		}
			
	}
	
	private void CablesOn()
	{
		foreach (var cable in _cables)
		{
			cable.Activate();
		}
	}

	private void CablesOff()
	{
		foreach (var cable in _cables)
		{
			cable.Deactivate();
		}
	}
}
