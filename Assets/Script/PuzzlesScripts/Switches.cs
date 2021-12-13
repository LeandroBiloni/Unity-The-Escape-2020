using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switches : MonoBehaviour
{
	[Header("Switches")]
	public Switches previusSwitch;
	public Switches nextSwitch;
	public bool activated;
	public GameObject parentOfSwitches;
	[Header("Doors")]
	public List<Door> doorsList = new List<Door>();
	public Door doorToOpen;
	public AudioClip stepSound;
	[Header("Timer")]
	public float maxTimeToUseAgain;
	[Header("Cables")]
	public Transform cableContainer;
	//public Transform[] myCables;
	public List<Cable> cables = new List<Cable>();
	private float _time;
	private bool _canActivate;
	public Color cablesDefaultColor;

	private void Start()
	{
		/*myCables = new Transform[cableContainer.childCount];
		for (int i = 0; i < myCables.Length; i++)
		{
			myCables[i] = cableContainer.GetChild(i);
			var myCable = myCables[i].gameObject.GetComponent<Cable>();
			cables.Add(myCable);
		}*/
	}
	private void Update()
	{
		if (activated)
		{
			MeshRenderer mesh = GetComponent<MeshRenderer>();
			mesh.material.color = Color.green;
			if (cables.Count != 0)
				CablesOn();
		}
		else
		{
			MeshRenderer mesh = GetComponent<MeshRenderer>();
			mesh.material.color = Color.red;
			if (cables.Count != 0)
				CablesOff();
		}
		
		if (_canActivate == false)
			ActivationTimer();
	}

	void Activation()
	{
		activated = true;
		if (doorToOpen != null)
			doorToOpen.activeSwitches++;
		Adjacent();
	}

	void Deactivate()
	{
		activated = false;
		if (doorToOpen != null)
			doorToOpen.activeSwitches--;
		Adjacent();
	}

	void Adjacent()
	{
		bool checkPreviusState = previusSwitch.activated;
		checkPreviusState = !checkPreviusState;
		previusSwitch.activated = checkPreviusState;

		bool checkNextState = nextSwitch.activated;
		checkNextState = !checkNextState;
		nextSwitch.activated = checkNextState;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && _canActivate)
		{
			print("nooooooooooooooooooooo");
			if (!activated)
				Activation();
			else
				Deactivate();
			if (doorsList.Count > 0)
			{
				doorsList[0].audioManager.PlaySFX(stepSound, 1.5f);

			}
			else
			{
				doorToOpen.audioManager.PlaySFX(stepSound, 1.5f);
			}

			CheckSwitches();
		}

		_canActivate = false;
	}

	void CheckSwitches()
	{
		List<Switches> allSwitches = new List<Switches>();
		for (int i = 0; i < parentOfSwitches.transform.childCount; i++)
		{
			allSwitches.Add(parentOfSwitches.transform.GetChild(i).GetComponent<Switches>());
		}
		//allSwitches.AddRange(FindObjectsOfType<Switches>());

		List<Switches> activatedSwitches = new List<Switches>();

		for (int i = 0; i < allSwitches.Count; i++)
		{
			if (allSwitches[i].activated)
			{
				activatedSwitches.Add(allSwitches[i]);
			}
			else
			{
				activatedSwitches.Remove(allSwitches[i]);
			}
		}

		if(activatedSwitches.Count == allSwitches.Count)
		{
			for (int i = 0; i < cables.Count; i++)
			{
				cables[i].activated = true;
			}

			for (int i = 0; i < doorsList.Count; i++)
			{
				doorToOpen = doorsList[i];
				doorToOpen.audioManager.PlaySFX(doorToOpen.disconectDoor);
				doorToOpen.openThroughComputer = true;
			}
		}
	}

	private void ActivationTimer()
	{
		_time += Time.deltaTime;

		if (_time >= maxTimeToUseAgain)
		{
			_canActivate = true;
			_time = 0;
		}

	}


	private void CablesOn()
	{
		foreach (var cable in cables)
		{
			var render = cable.gameObject.GetComponent<Renderer>();
			Material[] materials = render.materials;
			materials[0].color = Color.green;
		}
	}

	private void CablesOff()
	{
		foreach (var cable in cables)
		{
			var render = cable.gameObject.GetComponent<Renderer>();
			Material[] materials = render.materials;
			materials[0].color = cablesDefaultColor;
		}
	}
}
