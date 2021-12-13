using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AlarmGuardBrain : MonoBehaviour
{
    public AlarmGuardBody body;
	public Animator anim;
	public bool controlled = false;
    public float x, y;
    private GameManager _manager;
    public MeshRenderer render;
    private FieldOfView _fov;
    private Rigidbody _rb;
    private Vector3 _playerDir;
    public Vector3 spawnPoint;
    private bool _playerInFOV;
    private Vector3 _directionToMove;
    public Vector3 posToMove;
    public bool moveToPos;
    public float distanceToStop;
    private ScenesManager _sceneManager;
    public NavMeshAgent nav;
    public AnimationClip killAnim;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
        render = GetComponent<MeshRenderer>();
        _fov = GetComponent<FieldOfView>();
        _rb = GetComponent<Rigidbody>();
        moveToPos = true;
        _sceneManager = FindObjectOfType<ScenesManager>();
		anim = GetComponent<Animator>();
		anim.SetBool("Chasing", true);
        nav = GetComponent<NavMeshAgent>();
        nav.destination = posToMove;
	}

    // Update is called once per frame
    void Update()
    {
        if (controlled) //EL JUGADOR VA A CONTROLAR AL ENEMIGO
        {
            print("ESTOY CONTROLADOOOO");
            moveToPos = false;
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
			anim.SetFloat("VelX", x);
			anim.SetFloat("VelZ", y);
			
			CheckKeys();
        }

        PlayerInFOV();

        if (_playerInFOV)
		{
            MoveTowardsPlayer();
			
		}
		else
		{
			MoveTo();
		}

        if (controlled == false && moveToPos && _playerInFOV == false)
        {
            body.Move(_directionToMove);
            nav.destination = posToMove;
            CheckIfArrived();
        }
	}

    private void CheckKeys()
    {
        if (x != 0 || y != 0)
            body.ManualMovement();

        if (Input.GetKeyDown(KeyCode.C)) //DEVUELVO EL CONTROL A LOS PERSONAJES DEL JUGADOR
        {
            print("DEVOLVI CONTROL");
            _manager.ChangePlayerToEnemy(false);
        }
    }

    //Obtengo la dirección del waypoint al que tengo que ir
    private void MoveTo()
    {
		anim.SetBool("Chasing", false);
		_directionToMove = posToMove - transform.position;
		CheckIfArrived();
    }

    private void PlayerInFOV()
    {
        if (_fov.visibleTargets.Count > 0)
        {
            if (_fov.visibleTargets[0].layer == LayerMask.NameToLayer("Player"))
                _playerInFOV = true;
        }
        else _playerInFOV = false;
    }

    private void MoveTowardsPlayer()
    {
		anim.SetBool("Chasing", true);
		_playerDir = _fov.visibleTargets[0].transform.position - transform.position;
        nav.destination = _fov.visibleTargets[0].transform.position;
        body.FollowMovement(new Vector3(_playerDir.x, transform.position.y, _playerDir.z));
	}

    private void CheckIfArrived()
    {
        float distance = Vector3.Distance(transform.position, posToMove);
        if (distance <= distanceToStop)
        {
            moveToPos = false;
			anim.SetBool("Chasing", false);
			anim.SetFloat("VelZ", 0);
			if (posToMove == spawnPoint)
            {
                _manager.CloseDoors();
                Destroy(gameObject);
            }
        }
    }

	public void Return() 
    {
        posToMove = spawnPoint;
        nav.destination = posToMove;
        moveToPos = true;
	}

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			StartCoroutine(Punch());
		}
    }

	IEnumerator Punch()
	{
        body.moveSpeed = 0;
		anim.SetTrigger("Punch");
		yield return new WaitForSeconds(killAnim.length);
		_sceneManager.LoseScreen();
	}
}
