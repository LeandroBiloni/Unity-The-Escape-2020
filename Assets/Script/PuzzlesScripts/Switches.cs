using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switches : MonoBehaviour
{
	[Header("Switches")]
	[SerializeField] private Switches _previousSwitch;
	[SerializeField] private Switches _nextSwitch;
	private bool _active;
	public GameObject parentOfSwitches;
	[Header("Doors")]
	[SerializeField] private List<Door> _doorsList = new List<Door>();
	public AudioClip stepSound;
	[Header("Timer")]
	public float maxTimeToUseAgain;
	[Header("Cables")]
	public Transform cableContainer;
	private float _time;
	private bool _canActivate;
	public Color cablesDefaultColor;
	private Cable[] _cables;
	private MeshRenderer _meshRenderer;

	public delegate void Activation();

	public event Activation OnActivation;
	
	private void Start()
	{
		var childs = transform.GetComponentsInChildren<Cable>();
        
		_cables = childs;

		_meshRenderer = GetComponent<MeshRenderer>();
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
		UpdateAdjacentSwitches();
	}

	void DeactivateSwitch()
	{
		_active = false;
		UpdateAdjacentSwitches();
	}

	void UpdateAdjacentSwitches()
	{
		_previousSwitch.UpdateState();

		_nextSwitch.UpdateState();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && _canActivate)
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
			var render = cable.gameObject.GetComponent<Renderer>();
			Material[] materials = render.materials;
			materials[0].color = Color.green;
		}
	}

	private void CablesOff()
	{
		foreach (var cable in _cables)
		{
			var render = cable.gameObject.GetComponent<Renderer>();
			Material[] materials = render.materials;
			materials[0].color = cablesDefaultColor;
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
