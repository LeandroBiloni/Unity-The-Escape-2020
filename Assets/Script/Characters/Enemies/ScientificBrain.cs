using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScientificBrain : MonoBehaviour
{
    public ScientificBody body;
    public bool controlled = false;
    public float x, y;
    private GameObject _mngObj;
    private GameManager _manager;
    public MeshRenderer render;
    private FieldOfView _fov;
    private Rigidbody _rb;
    private bool _canMove;
    public GameObject particle;

    //WAYPOINTS
    public List<Transform> waypointsContainer = new List<Transform>(); //AGREGAR CANTIDAD DE WAYPOINTS QUE VA A TENER EN EL INSPECTOR!!!!!
    public bool hasWp; //MARCAR EN INSPECTOR SI EL ENEMIGO VA A TENER WAYPOINTS O NO
    public Vector3 _nextWP;
    private int _WpContainerIndex;
    private bool _canSum = true;
    public Vector3 waypointDir;

    //PUNTO PARA ESCONDERSE
    public GameObject hideLocation;
    public bool willHide;

    private bool _playerInFOV;
    public Vector3 playerPos;
    public bool _moveToPos;
    public Vector3 _posToMove;

    private Alarm _alarm;
    public NavMeshAgent nav;
	public GameObject eCommand;
    private bool _alarmReached;
    public bool canUseAlarms;
    public float resetBehaviourTime;
    private float _time;
	bool scared;

	// Start is called before the first frame update
	void Start()
    {
        body = GetComponent<ScientificBody>();
        _mngObj = GameObject.Find("GameManager");
        _manager = _mngObj.GetComponent<GameManager>();
        if (hasWp)
            _nextWP = waypointsContainer[0].position;
        render = GetComponent<MeshRenderer>();
        _fov = GetComponent<FieldOfView>();
        _rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (controlled) //EL JUGADOR VA A CONTROLAR AL ENEMIGO
        {
            body.moveToAlarm = false;
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
			body.anim.SetFloat("VelX", Input.GetAxis("Horizontal"));
			body.anim.SetFloat("VelZ", Input.GetAxis("Vertical"));
            CheckKeys();
        }
        else
        {
			PlayerInFOV();

            WaypointDirection();

            if (_playerInFOV && canUseAlarms)
                GetAlarm();
            if (_playerInFOV && willHide)
            {
				scared = willHide;
                //PONER ACA REPRODUCCION DE ANIMACION DE ASUSTADO
                if (hasWp)
                    hasWp = false;
            }

            else if (hasWp && !controlled)
            {
                body.WaypointMovement(waypointDir);
                DistanceToWP();
            }

            if (_alarmReached)
            {
                _time += Time.deltaTime;
                if (_time >= resetBehaviourTime)
                {
                    ResetBehaviour();
                }
            }
        }

		if(scared && !controlled)
			body.anim.SetBool("Dizzy", true);
	}

	private void CheckKeys()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            _canMove = true;
			body.anim.SetBool("Dizzy", false);
        }
        else
        {
            _canMove = false;
            x = 0;
            y = 0;
        }

        if (_canMove && (x != 0 || y != 0))
        {
            _rb.isKinematic = false;
            body.ManualMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space)) //DEVUELVO EL CONTROL A LOS PERSONAJES DEL JUGADOR
        {
			body.anim.SetBool("Dizzy", false);
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
            {
                _playerInFOV = true;
                playerPos = _fov.visibleTargets[0].transform.position;
            }                
        }
        else _playerInFOV = false;
    }

    private void GetAlarm()
    {
        hasWp = false;
        _posToMove = _manager.ClosestAlarm(this.transform) - transform.position;
        body.goToPos = _posToMove;
        nav.destination = _manager.ClosestAlarm(this.transform);
        body.moveToAlarm = true;
    }

    public void DistanceToWP()
    {
        float distance = Vector3.Distance(transform.position, _nextWP);
        if (distance <= 1f)
        {
            SelectWaypoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Alarm") && !_alarmReached && !canUseAlarms) 
        {
            _alarm = other.GetComponent<Alarm>();
            _alarm.playerPos = playerPos;
            body.moveToAlarm = false;
            _alarmReached = true;
        }
    }

    private void ResetBehaviour()
    {
        _alarmReached = false;
        hasWp = true;
        _time = 0;
    }
}
