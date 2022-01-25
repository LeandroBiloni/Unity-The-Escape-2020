using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	
	float _mass;
	private Vector3 _origScale;
	private Vector3 _childScale;
	private GameObject _empty;

	private void OnCollisionEnter(Collision collision)
	{
		
		if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			//_origScale = collision.transform.localScale;
			//collision.transform.localScale = new Vector3(collision.transform.localScale.x * (collision.transform.localScale.x/transform.localScale.x), collision.transform.localScale.y * (collision.transform.localScale.y/transform.localScale.y), collision.transform.localScale.z * (collision.transform.localScale.z / transform.localScale.z));
			_mass = collision.rigidbody.mass;
			if (!_empty)
				_empty = new GameObject();
			
			_empty.transform.parent = transform;
			collision.transform.parent = _empty.transform;
			_mass = collision.rigidbody.mass;
			collision.rigidbody.mass = 0f;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			collision.transform.parent = null;
			collision.rigidbody.mass = _mass;
		}
	}
}
