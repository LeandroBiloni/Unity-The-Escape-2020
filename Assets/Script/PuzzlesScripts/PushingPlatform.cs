using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class PushingPlatform : MonoBehaviour
{
	public GameObject platform;
	public Transform moveHere;
	public bool activated;
	Vector3 startPostion;

	float speed = 1.5f;

	private void Start()
	{
		startPostion = platform.transform.position;
	}

	private void Update()
	{
		if (activated)
		{
			Move();
		}
		else
		{
			MoveBack();
		}
	}

	void Move()
	{
		float distance = Vector3.Distance(platform.transform.position, moveHere.position);
		if(distance > 0.1f)
		{
			Vector3 dist = moveHere.position - platform.transform.position;
			platform.transform.position += dist.normalized * speed * Time.deltaTime;
			
		}
		else if (distance <= 0.1f)
		{
			platform.transform.position = moveHere.position;
		}
	}

	private void MoveBack()
	{
		float distance = Vector3.Distance(platform.transform.position, startPostion);
		if (distance > 0.1f)
		{
			Vector3 dist = startPostion - platform.transform.position;
			platform.transform.position += dist.normalized * speed * Time.deltaTime;

		}
		else if (distance <= 0.1f)
		{
			platform.transform.position = startPostion;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
		{
			activated = true;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
		{
			activated = false;
		}
	}
}
