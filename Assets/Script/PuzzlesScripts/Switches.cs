using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switches : MonoBehaviour
{
	[SerializeField] private bool _active;
	[Header("Switches")] 
	[SerializeField] private List<Switches> _switchesToInteract;
	[Header("Doors")]
	[SerializeField] private List<Door> _doorsList = new List<Door>();
	public AudioClip stepSound;
	private float _time;
	private bool _canInteract;
	[SerializeField] private List<Cable> _cables = new List<Cable>();
	private MeshRenderer _meshRenderer;

	public delegate void Activation();

	public event Activation OnActivation;
	public event Activation OnDeactivation;

	private void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.material.color = Color.red;
		_canInteract = true;
		
		if (_doorsList.Count > 0)
		{
			foreach (var door in _doorsList)
			{
				door.AddSwitch(this);
			}
		}
	}

	private void Start()
	{
		if (_active)
		{
			_active = true;
			_meshRenderer.material.color = Color.green;
			CablesOn();
			OnActivation?.Invoke();
		}
	}

	void ActivateSwitch()
	{
		_active = true;
		_meshRenderer.material.color = Color.green;
		UpdateAdjacentSwitches();
		CablesOn();
		OnActivation?.Invoke();
	}

	void DeactivateSwitch()
	{
		_active = false;
		_meshRenderer.material.color = Color.red;
		UpdateAdjacentSwitches();
		CablesOff();
		OnDeactivation?.Invoke();
	}

	void UpdateAdjacentSwitches()
	{
		foreach (var sw in _switchesToInteract)
		{
			sw.UpdateState();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player") && _canInteract)
		{
			if (!_active)
				ActivateSwitch();
			else DeactivateSwitch();
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

	public bool IsActive()
	{
		return _active;
	}

	private void UpdateState()
	{
		_active = !_active;
		
		if (_active)
		{
			_meshRenderer.material.color = Color.green;
			CablesOn();
			OnActivation?.Invoke();
		}
		else
		{
			_meshRenderer.material.color = Color.red;
			CablesOff();
			OnDeactivation?.Invoke();
		}
	}

	public void DeactivateInteraction()
	{
		_canInteract = false;
	}
}
