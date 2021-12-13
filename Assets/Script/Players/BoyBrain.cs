using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBrain : MonoBehaviour
{
    public BoyBody body;
	public GameManager manager;
	public Transform powerPoint;
	public Transform _currentObject;
	[HideInInspector]
	public Rigidbody _currentObjectRB;
	public AudioClip throwBoxSound;
	public AudioClip pullBoxSound;
	public bool _enemyHit;

	public int _targetIndex;
	public float horizontal, vertical;
	public float force = 10f;
	public float cooldownTime = 1.1f;
    public bool selected;
	public bool objectsInFov;
	public static bool checkpointReached;

	[HideInInspector]
	public bool holding = false;
	public bool canMove;

	Rigidbody _rb;
	FieldOfView _myFoV;
	Animator _anim;
	public CheckpointManager _checkManager;
	bool _pulling = false;
	bool _canUsePower = true;
	float _currentTime;


	// Start is called before the first frame update
	void Start()
	{
		body = GetComponent<BoyBody>();
		_myFoV = GetComponent<FieldOfView>();
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
		_checkManager = FindObjectOfType<CheckpointManager>();
		if (manager.checkpointsReached)
		{
			print("hello");
			transform.position = _checkManager.boyCheckpointPos;
		}

	}

    // Update is called once per frame
    void Update()
    {
		
		if (selected && _enemyHit == false)
        {
			_anim.SetFloat("VelZ", Input.GetAxis("Vertical"));
			_anim.SetFloat("VelX", Input.GetAxis("Horizontal"));
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			CheckKeys();

			CheckObjectInFOV();
		}
		else
		{
			_anim.SetFloat("VelZ", 0);
			_anim.SetFloat("VelX", 0);
		}

	
		if (holding)
		{
			HoldObject();
			if (_rb.velocity.magnitude > 1)
			{
				_rb.velocity = new Vector3(1, 0, 1);
			}

			if (horizontal == 0 && vertical == 0)
				_rb.velocity = Vector3.zero;

		}
	
		if (_currentTime < cooldownTime)
		{
			CooldownTimer();
		}
	}


	private void CheckKeys()
	{
		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
		{
			canMove = true;
		}
		else
		{
			canMove = false;
			horizontal = 0;
			vertical = 0;
		}

		if (canMove && ((horizontal != 0 && !_pulling) || (vertical != 0 && !_pulling)))
		{
			/*int stepSound = 0;
			if (stepSound == 0)
			{
				manager.audioManager.PlaySFX(manager.step1);
				stepSound++;
			}
			else
			{
				manager.audioManager.PlaySFX(manager.step2);
				stepSound = 0;
			}*/

			_rb.isKinematic = false;
			body.Movement();
		}
		

		if (Input.GetKeyDown(KeyCode.E) && objectsInFov && !holding && _canUsePower) //Atrae el objeto
		{

			_pulling = true;
			manager.audioManager.PlaySFX(pullBoxSound);
			_anim.SetTrigger("poder");
			StartCoroutine("Telekinesis");
		}

		if(holding && Input.GetKeyDown(KeyCode.Space)) //Lanza el objeto
		{
			manager.audioManager.PlaySFX(throwBoxSound, .5f);
			ThrowObject();
		}

		if(holding && Input.GetKeyDown(KeyCode.Q)) //Suelta el objeto
		{
			_currentObjectRB = _currentObject.GetComponent<Rigidbody>();
			_currentObjectRB.constraints = RigidbodyConstraints.None;
			holding = false;
		}

		if (objectsInFov)
		{
			if (Input.GetKeyDown(KeyCode.Tab))
				NextTarget();
		}
    }

	IEnumerator Telekinesis()  //Atrae a un objeto
	{
		if (objectsInFov)
		{
			Debug.Log("Pulling");
			_currentObject = _myFoV.visibleTargets[_targetIndex].transform;  //asigno cual es el objeto
			while (_pulling)
			{

				Vector3 direction = (powerPoint.position - _currentObject.transform.position);
				_anim.SetFloat("VelZ", 0);
				_anim.SetFloat("VelX", 0);

				if (Vector3.Distance(_currentObject.transform.position, powerPoint.position) >= 0)
				{
					_currentObject.transform.position = _currentObject.transform.position  + direction * force * Time.deltaTime; //Esto Atrae al objeto
					
					if (Vector3.Distance(_currentObject.transform.position, powerPoint.position) <= 0.5) // para pasar de arrastrar a agarrar
					{
						Debug.Log("Holding");
						holding = true;
					}
					yield return new WaitForSecondsRealtime(.01f);
				}
			}
		}
	}
	
	void HoldObject()
	{
		_pulling = false;
		_currentObject.transform.position = powerPoint.position;
		_currentObjectRB = _currentObject.GetComponent<Rigidbody>();
		_currentObjectRB.constraints = RigidbodyConstraints.FreezePositionY;
	}

	void ThrowObject()
	{
		_anim.SetTrigger("poder");
		
		holding = false;
		
		_currentObjectRB = _currentObject.GetComponent<Rigidbody>();
		_currentObjectRB.constraints = RigidbodyConstraints.None;
		_currentObjectRB.AddForce(powerPoint.forward * 10f, ForceMode.VelocityChange);  //Lanza el objeto hacia donde esta mirando

		_currentTime = 0f;
	}

	void CooldownTimer() //Se llama despues de tirar un objeto, lo implemente para evitar un error
	{
		_currentTime += Time.deltaTime;
		if(_currentTime >= cooldownTime)
			_canUsePower = true;
		else
			_canUsePower = false;
	} 

	void CheckObjectInFOV()
	{
		if (_myFoV.visibleTargets.Count == 0) //Checkea si hay objetos a la vista
		{
			objectsInFov = false;
			_targetIndex = 0;
			manager.ObjectInPlayerFOV(objectsInFov, null);
		}
		else
		{
			objectsInFov = true;
			if (_myFoV.visibleTargets.Count == 1)
				_targetIndex = 0;
			manager.ObjectInPlayerFOV(objectsInFov, _myFoV.visibleTargets[_targetIndex]);
		}
	}

	void NextTarget()
	{
		_targetIndex++;
		if (_targetIndex == _myFoV.visibleTargets.Count)
		{
			_targetIndex = 0;
		}
		manager.ObjectInPlayerFOV(objectsInFov, _myFoV.visibleTargets[_targetIndex]);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Checkpoint2"))
		{
			checkpointReached = true;
			_checkManager.boyCheckpointPos = other.transform.position;           
            _checkManager.active1 = true;
        }      
    }

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Guard") || collision.gameObject.CompareTag("Patrol"))
		{
			_enemyHit = true;
		}
	}
}
