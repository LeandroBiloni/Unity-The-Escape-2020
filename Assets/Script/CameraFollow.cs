using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform characterToFollow;
	public GameManager manager;
	public float smoothSpeed = 5;
	public float moveSpeed;
	public Vector3 offset;
	public bool follow;
	

	private void Start()
	{
		follow = true;
	}

	private void Update()
	{
		/*if (manager.isChanged == false)
		{
			characterToFollow = manager.boy.transform;
		}
			
		else if (manager.isChanged)
		{
			if (manager.girl.selected)
				characterToFollow = manager.girl.transform;
		}*/
			
	}

	private void LateUpdate()
	{
		Vector3 desiredPosition = characterToFollow.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		if (follow == true)
		{
			transform.position = smoothedPosition;
		}
		else CameraMovement();
	}

	public void CameraMovement()
	{
		follow = false;
		
		float distanceToTarget = Vector3.Distance(characterToFollow.position, transform.position);

		if (transform.position != characterToFollow.position)
			Vector3.MoveTowards(transform.position, characterToFollow.position, distanceToTarget);
		else if ((transform.position.z - characterToFollow.position.z) < 0.1f || (transform.position.x - characterToFollow.position.x) < 0.1f)
			follow = true; 
	}
}
