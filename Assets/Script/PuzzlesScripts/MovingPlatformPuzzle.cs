using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPuzzle : MonoBehaviour
{
	public GameObject platform;
	public bool moveForward;
	public bool right;
	public bool forward;
	public float speed = 1f;

	[SerializeField]
	bool _activated;

	private void Update()
	{
		if (_activated && !moveForward)
		{
			MoveRight();
		}
		else if (_activated && moveForward)
		{
			MoveForward();
		}
	}

	void MoveRight()
	{
		if (right)
			platform.transform.position += Vector3.right * speed * Time.deltaTime;
		else
			platform.transform.position += -Vector3.right * speed * Time.deltaTime;
	}

	void MoveForward()
	{
		if (forward)
			platform.transform.position += Vector3.forward * speed * Time.deltaTime;
		else
			platform.transform.position += -Vector3.forward * speed * Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			_activated = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			_activated = false;
	}
}
