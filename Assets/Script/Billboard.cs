using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Transform _cam;

	private void Start()
	{
		_cam = GameObject.Find("Main Camera").transform;
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void LateUpdate()
    {
		transform.LookAt(transform.position + _cam.forward);
    }
}
