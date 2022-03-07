using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
	ScenesManager scenesManager;
	private void Start()
	{
		scenesManager = FindObjectOfType<ScenesManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Debug.Log("Se cayo el player");
			scenesManager.LoseScreen();
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Debug.Log("Se cayo el enemigo");

			scenesManager.LoseScreen();
		}

	}
}
