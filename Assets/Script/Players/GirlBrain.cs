using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlBrain : MonoBehaviour
{
    public GirlBody body;
    public float horizontal, vertical;
    public bool selected;
    public GameManager manager;
    public bool controllingEnemy = false;
    public float timer = 0;
    public float maxTime;
    public bool startTimer = false;
    public float viewRadius = 360;
    public FieldOfView fov;
    public bool enemyInFOV;
    public int targetIndex = 0;
    public bool canMove;
	public static bool checkpointReched;
    private Rigidbody _rb;
	private Animator _anim;
	private CheckpointManager _checkManager;
    public GameObject commandIcon;
    public bool _enemyHit;

	public Image powerBar;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<GirlBody>();
        fov = GetComponent<FieldOfView>();
        selected = false;
        _rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
		_checkManager = FindObjectOfType<CheckpointManager>();

		if (checkpointReched)
			transform.position = _checkManager.girlCheckpointPos;
        commandIcon.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        

        if (selected && controllingEnemy == false && _enemyHit == false)
        {
            _anim.SetFloat("VelZ", Input.GetAxis("Vertical"));
            _anim.SetFloat("VelX", Input.GetAxis("Horizontal"));
            CheckEnemyInFOV();
            CheckKeys();
        }
		else
		{
			_anim.SetFloat("VelZ", 0);
			_anim.SetFloat("VelX", 0);
		}

        if (startTimer)
        {
            ControlTimer();
        }

        if (controllingEnemy)
            commandIcon.SetActive(true);
        else commandIcon.SetActive(false);
    }


    private void CheckKeys()
    {
        if (controllingEnemy == false)
        {

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                //canMove = true;
                _rb.isKinematic = false;

                int stepSound = 0;
                if (stepSound == 0)
                {
                    manager.audioManager.PlaySFX(manager.step1);
                    stepSound++;
                }
                else
                {
                    manager.audioManager.PlaySFX(manager.step2);
                    stepSound = 0;
                }

                body.Movement();
            }
            else
            {
                canMove = false;
                horizontal = 0;
                vertical = 0;
            }

            /*if (canMove && (horizontal != 0 || vertical != 0))
            {
                _rb.isKinematic = false;

			    int stepSound = 0;
			    if (stepSound == 0)
			    {
				    manager.audioManager.PlaySFX(manager.step1);
				    stepSound++;
			    }
			    else
			    {
				    manager.audioManager.PlaySFX(manager.step2);
				    stepSound = 0;
			    }

			    body.Movement();
            }*/

            if (Input.GetKeyDown(KeyCode.E) && enemyInFOV)
            {
                ControlEnemy();
                _anim.SetTrigger("Poder");
            }

            if (enemyInFOV)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                    NextTarget();
            }
        }

        if (controllingEnemy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                CancelControl();
        }

            
    }

    private void ControlEnemy()
    {
        if (controllingEnemy == false)
        {
            startTimer = true;
            manager.ChangePlayerToEnemy(true); //LLAMA AL GameManager PARA QUE AVISE AL EnemyBrain QUE ESTA SIENDO CONTROLADO
            controllingEnemy = true;
        }
    }
    private void CancelControl()
    {
        if (controllingEnemy)
        {
            manager.ChangePlayerToEnemy(false); //CANCELA EL CONTROL DEL EnemyBrain
            timer = 0;
			powerBar.fillAmount = 1;
			controllingEnemy = false;
            startTimer = false;
        }
    }

    //TIMER-COOLDOWN DE LA FUNCION ControlEnemy
    public void ControlTimer()
    {
        print("empezo el timer");
        if (timer < maxTime)
        {
			if(controllingEnemy)
				powerBar.fillAmount = timer / maxTime;

            timer += Time.deltaTime;
        }
        
        if (timer >= maxTime)
        {
            timer = 0;
			powerBar.fillAmount = 1;
			startTimer = false;
            controllingEnemy = false;
            manager.controllingEnemy = false;
            manager.ChangePlayerToEnemy(false);
        }
    }

    void CheckEnemyInFOV()
    {
        if (fov.visibleTargets.Count == 0) //Checkea si hay enemigos a la vista
        {
            enemyInFOV = false;
            targetIndex = 0;
            print("no enemy");
            manager.EnemyInPlayerFOV(enemyInFOV, null, "");
        }
        else
        {
            enemyInFOV = true;
            if (fov.visibleTargets.Count == 1)
                targetIndex = 0;
            print("enemigo en fov: " + fov.visibleTargets[targetIndex].name);
            manager.EnemyInPlayerFOV(enemyInFOV, fov.visibleTargets[targetIndex], fov.visibleTargets[targetIndex].tag);
        }
    }

    void NextTarget()
    {

        targetIndex++;
        if (targetIndex == fov.visibleTargets.Count)
        {
            targetIndex = 0;
        }
        manager.EnemyInPlayerFOV(enemyInFOV, fov.visibleTargets[targetIndex], fov.visibleTargets[targetIndex].tag);
    }

	private void OnTriggerEnter(Collider other)
	{
		
        if (other.CompareTag("Checkpoint"))
        {
            checkpointReched = true;
            manager.checkpointsReached = checkpointReched;
            _checkManager.girlCheckpointPos = other.transform.position;            
            _checkManager.active2 = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Guard") || collision.gameObject.CompareTag("Patrol"))
        {
            _enemyHit = true;
        }
    }
}
