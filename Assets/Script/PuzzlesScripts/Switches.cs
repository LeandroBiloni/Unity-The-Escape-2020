using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switches : MonoBehaviour
{
	[Header("Switches")] 
	[SerializeField] private List<Switches> _switchesToInteract;
	private bool _active;
	[Header("Doors")]
	[SerializeField] private List<Door> _doorsList = new List<Door>();
	public AudioClip stepSound;
	private float _time;
	private bool _canActivate;
	[SerializeField] private List<Cable> _cables = new List<Cable>();
	private MeshRenderer _meshRenderer;

	public delegate void Activation();

	public event Activation OnActivation;
	
	private void Start()
	{
		_meshRenderer = GetComponent<MeshRenderer>();

		if (_doorsList.Count > 0)
		{
			foreach (var door in _doorsList)
			{
				door.AddSwitch(this);
			}
		}
	}
	private void Update()
	{
		// if (_active)
		// {
		// 	MeshRenderer mesh = GetComponent<MeshRenderer>();
		// 	mesh.material.color = Color.green;
		// 	if (cables.Count != 0)
		// 		CablesOn();
		// }
		// else
		// {
		// 	MeshRenderer mesh = GetComponent<MeshRenderer>();
		// 	mesh.material.color = Color.red;
		// 	if (cables.Count != 0)
		// 		CablesOff();
		// }
		//
		// if (_canActivate == false)
		// 	ActivationTimer();
	}

	void ActivateSwitch()
	{
		_active = true;
		
		OnActivation?.Invoke();
		
		CablesOn();
		
		UpdateAdjacentSwitches();
	}

	void DeactivateSwitch()
	{
		_active = false;
		
		CablesOff();
		
		UpdateAdjacentSwitches();
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
		if(other.gameObject.layer == LayerMask.NameToLayer("Player") && _canActivate)
		{
			if (!_active)
				ActivateSwitch();
			else DeactivateSwitch();
			
			// if (_doorsList.Count > 0)
			// {
			// 	_doorsList[0].audioManager.PlaySFX(stepSound, 1.5f);
			//
			// }
			// else
			// {
			// 	doorToOpen.audioManager.PlaySFX(stepSound, 1.5f);
			// }
		}
	}

	// void CheckSwitches()
	// {
	// 	List<Switches> allSwitches = new List<Switches>();
	// 	for (int i = 0; i < parentOfSwitches.transform.childCount; i++)
	// 	{
	// 		allSwitches.Add(parentOfSwitches.transform.GetChild(i).GetComponent<Switches>());
	// 	}
	// 	//allSwitches.AddRange(FindObjectsOfType<Switches>());
	//
	// 	List<Switches> activatedSwitches = new List<Switches>();
	//
	// 	for (int i = 0; i < allSwitches.Count; i++)
	// 	{
	// 		if (allSwitches[i]._active)
	// 		{
	// 			activatedSwitches.Add(allSwitches[i]);
	// 		}
	// 		else
	// 		{
	// 			activatedSwitches.Remove(allSwitches[i]);
	// 		}
	// 	}
	//
	// 	if(activatedSwitches.Count == allSwitches.Count)
	// 	{
	// 		for (int i = 0; i < cables.Count; i++)
	// 		{
	// 			cables[i].activated = true;
	// 		}
	//
	// 		for (int i = 0; i < _doorsList.Count; i++)
	// 		{
	// 			doorToOpen = _doorsList[i];
	// 			doorToOpen.audioManager.PlaySFX(doorToOpen.disconectDoor);
	// 			doorToOpen.openThroughComputer = true;
	// 		}
	// 	}
	// }

	// private void ActivationTimer()
	// {
	// 	_time += Time.deltaTime;
	//
	// 	if (_time >= maxTimeToUseAgain)
	// 	{
	// 		_canActivate = true;
	// 		_time = 0;
	// 	}
	//
	// }


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
		
		if (_active) OnActivation?.Invoke();
	}
}
