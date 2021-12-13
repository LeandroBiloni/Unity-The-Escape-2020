using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesChica : MonoBehaviour
{
	public SwitchesChica previusSwitch;
	public SwitchesChica nextSwitch;
	public bool activated;
	public GameObject parentOfSwitches;
	public List<Door> doorsList = new List<Door>();
	public Door doorToOpen;
	public AudioClip stepSound;
	public float maxTimeToUseAgain;
	private float _time;
	private bool _canActivate;


	private void Update()
	{
		if (activated)
		{
			MeshRenderer mesh = GetComponent<MeshRenderer>();
			mesh.material.color = Color.green;
		}
		else
		{
			MeshRenderer mesh = GetComponent<MeshRenderer>();
			mesh.material.color = Color.cyan;
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
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
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

		if (activatedSwitches.Count == allSwitches.Count)
		{
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
}