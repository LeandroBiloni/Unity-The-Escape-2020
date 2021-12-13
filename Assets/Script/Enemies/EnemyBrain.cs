using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    public EnemyBody body;
    public bool controlled = false;
    public float x, y;
    private GameObject _mngObj;
    private GameManager _manager;
    public MeshRenderer render;
    private FieldOfView _fov;
    private Rigidbody _rb;
    public bool canMove;
    public GameObject particle;
    //WAYPOINTS
    public List<Transform> waypointsContainer = new List<Transform>(); //AGREGAR CANTIDAD DE WAYPOINTS QUE VA A TENER EN EL INSPECTOR!!!!!
    public bool hasWp; //MARCAR EN INSPECTOR SI EL ENEMIGO VA A TENER WAYPOINTS O NO
    public Vector3 _nextWP;
    private int _WpContainerIndex;
    private bool _canSum = true;
    public Vector3 waypointDir;

    private bool _playerInFOV;
    private Vector3 _playerDir;
    public bool _moveToPos;
    public Vector3 _posToMove;
    public NavMeshAgent nav;
    private ScenesManager _sceneManager;
    public GameObject eCommand;
    public AnimationClip killAnim;

    // Start is called before the first frame update
    void Start()
    {
        _mngObj = GameObject.Find("GameManager");
        body = GetComponent<EnemyBody>();
        _manager = _mngObj.GetComponent<GameManager>();
        if (hasWp)
            _nextWP = waypointsContainer[0].position;
        render = GetComponent<MeshRenderer>();
        _fov = GetComponent<FieldOfView>();
        _rb = GetComponent<Rigidbody>();
        _sceneManager = FindObjectOfType<ScenesManager>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlled) //EL JUGADOR VA A CONTROLAR AL ENEMIGO
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            CheckKeys();
        }
        else
        {
            PlayerInFOV();

            WaypointDirection();

            if (_playerInFOV && !controlled)
                MoveTowardsPlayer();
            else if (hasWp && !controlled)
            {
                body.anim.SetBool("Chasing", false);
                body.WaypointMovement(waypointDir);
                DistanceToWP();
            }

            if (_moveToPos)
                CheckIfArrived();
        }
        
    }

    private void CheckKeys()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
			body.anim.SetBool("Dizzy", false);
			canMove = true;
        }
        else
        {
            canMove = false;
            x = 0;
            y = 0;
        }

        if (canMove && (x != 0 || y != 0))
        {
            _rb.isKinematic = false;
            body.ManualMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space)) //DEVUELVO EL CONTROL A LOS PERSONAJES DEL JUGADOR
        {
            print("APRETE C CIENTIFICO");
            controlled = false;
            _manager.controllingEnemy = false;
            _manager.ChangePlayerToEnemy(false);
        }
    }

    //Selecciono el waypoint al que tengo que ir
    public void SelectWaypoint()
    {
        if (_WpContainerIndex == waypointsContainer.Count - 1)
            _canSum = false;

        if (_WpContainerIndex == 0)
            _canSum = true;
        if (_canSum)
            _WpContainerIndex++;
        else _WpContainerIndex--;

        _nextWP = waypointsContainer[_WpContainerIndex].transform.position;
        nav.destination = _nextWP;
    }

    //Obtengo la dirección del waypoint al que tengo que ir
    private void WaypointDirection()
    {
        waypointDir = _nextWP - transform.position;
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
		body.anim.SetBool("Chasing", true);
        _playerDir = _fov.visibleTargets[0].transform.position - transform.position;
        nav.destination = _fov.visibleTargets[0].transform.position;
        body.FollowMovement(new Vector3(_playerDir.x,transform.position.y,_playerDir.z));
    }

    public void PlayerSeenPosition(Vector3 playerPos)
    {
        if (body.moveToPosition == false)
        {
            body.moveToPosition = true;
            body.collideWithWP = false;
            _moveToPos = true;
            _posToMove = playerPos;
            Vector3 dir = _posToMove - transform.position;
            body.goToPos = dir;
        }           
    }

    private void CheckIfArrived()
    {
        float distance = Vector3.Distance(transform.position, _posToMove);
        if (distance <= 1f)
        {
            body.moveToPosition = false;
			body.anim.SetBool("Chasing", false);
            _moveToPos = false;

            if (body.collideWithWP == true)
                hasWp = true;
        }
    }

    public void DistanceToWP()
    {
        float distance = Vector3.Distance(transform.position, _nextWP);
        if (distance <= 1f)
        {
            SelectWaypoint();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
			StartCoroutine(Punch());
    }

	IEnumerator Punch()
	{
		body.anim.SetTrigger("Punch");
        body.moveSpeed = 0;
        yield return new WaitForSeconds(killAnim.length);
        _sceneManager.LoseScreen();

	}
}
