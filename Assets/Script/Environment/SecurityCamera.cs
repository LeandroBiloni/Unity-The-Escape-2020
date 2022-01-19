using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
	public float minRotation;
	public float maxRotation;
	public float stopTime;
	public bool targetInFoV;
	int targetIndex = 0;
	Vector3 _rotation;

	Rigidbody _rb;
	Animator _anim;
	FieldOfView _cameraFieldOfView;
	Transform _target;
	GameManager _manager;

	private bool _gotTargetPos;
	
    // Start is called before the first frame update
    void Start()
    {
		_manager = FindObjectOfType<GameManager>();
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
		_cameraFieldOfView = GetComponentInChildren<FieldOfView>();
		//_rotation = new Vector3(0, maxRotation, 0);
	}

	private void Update()
	{
		/*if (gameObject.transform.rotation.y == maxRotation || gameObject.transform.rotation.y == minRotation)
			Stop();*/
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		CheckFov();
		if (targetInFoV)
		{
			_anim.SetBool("Following", true);
			FollowTarget();
			//if (_gotTargetPos == false)
				//GetPlayerPosition();
		}
			
		else
			Rotate();
	}

	void Rotate()
	{
		if (!targetInFoV)
		{
			_anim.SetBool("Following", false);
			//Quaternion deltaRotation = Quaternion.Euler(_rotation);
			//_rb.MoveRotation(_rb.rotation * deltaRotation);
			//_rb.rotation *= deltaRotation;
		}
	}

	/*
	void ChangeRotation()
	{
		if (_rotation.y == maxRotation)
			_rotation = -Vector3.up;
		else if (_rotation.y == minRotation)
			_rotation = Vector3.up;
	}*/

	void CheckFov()
	{
		if (_cameraFieldOfView.visibleTargets.Count > 0)
		{
			if (_cameraFieldOfView.visibleTargets[0].layer == LayerMask.NameToLayer("Player"))
			{
				print("PLAYER IN FOV");
				targetInFoV = true;
				GetPlayerPosition();
			}
		}
		else 
		{
			targetInFoV = false;
			_gotTargetPos = false;
		}	
	}

	void FollowTarget() 
	{
		_target = _cameraFieldOfView.visibleTargets[targetIndex].transform;
		transform.LookAt(_target);
	}


	/*private void OnTriggerEnter(Collider other)
	{
		if(!targetInFoV)
			ChangeRotation();
	}*/
	
	private void GetPlayerPosition()
	{
		_gotTargetPos = true;
	}
}
