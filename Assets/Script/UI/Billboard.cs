using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Transform _cam;

	private void Start()
	{
		var cam = FindObjectOfType<CameraFollow>();
		
		if (cam)
			_cam = cam.transform;
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void LateUpdate()
    {
	    if (!_cam)
	    {
		    _cam = FindObjectOfType<CameraFollow>().transform;
	    }
		transform.LookAt(transform.position + _cam.forward);
    }
}
