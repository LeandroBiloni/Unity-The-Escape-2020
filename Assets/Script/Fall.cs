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
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			scenesManager.LoseScreen();
	}
}
