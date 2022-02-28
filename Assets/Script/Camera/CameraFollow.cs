using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform _characterToFollow;
	public float smoothSpeed = 5;
	public float moveSpeed;
	public Vector3 offset;
	public bool follow;
	

	private void Start()
	{
		follow = true;
		var selector = FindObjectOfType<CharacterSelector>();
		selector.OnFocusChange += SetTarget;

		var girl = FindObjectOfType<Girl>();
		girl.OnControlChange += SetTarget;

		if (selector)
		{
			var character = selector.GetSelectedCharacter();

			if (character)
			{
				_characterToFollow = character.transform;
			}
		}
		
	}

	private void LateUpdate()
	{
		if (!_characterToFollow)
		{
			var selector = FindObjectOfType<CharacterSelector>();
			_characterToFollow = selector.GetSelectedCharacter().transform;
		}
		else
		{
			Vector3 desiredPosition = _characterToFollow.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
			if (follow == true)
			{
				transform.position = smoothedPosition;
			}
			else CameraMovement();
		}
		
	}

	public void CameraMovement()
	{
		follow = false;
		
		float distanceToTarget = Vector3.Distance(_characterToFollow.position, transform.position);

		if (transform.position != _characterToFollow.position)
			Vector3.MoveTowards(transform.position, _characterToFollow.position, distanceToTarget);
		else if ((transform.position.z - _characterToFollow.position.z) < 0.1f || (transform.position.x - _characterToFollow.position.x) < 0.1f)
			follow = true; 
	}

	public void SetTarget(GameObject target)
	{
		_characterToFollow = target.transform;
	}
}
