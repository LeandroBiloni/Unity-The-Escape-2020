using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
	//ENEMIES
	[Header("ENEMIES")]
	public List<GameObject> _enemiesList = new List<GameObject>();
	private GameObject _closestEnemy;
	public GameObject _enemiesContainer;
	public EnemyBrain enemyToControl;
	public ScientificBrain scientificToControl;
	public AlarmGuardBrain guardToControl;
	public List<GameObject> roomsList = new List<GameObject>();
	public List<GameObject> _spawnsList = new List<GameObject>();
	public Transform enemyPos;
	public bool controllingEnemy;
	public int _enemyToSelect; // 1 = patrol ; 2 = scientific ; 3 = guard ; 0 = nada
	private bool _alarmSpawned;
	private bool _cameraSpawned;
	private GuardSpawn _cameraSpawnBoySide;
	private GuardSpawn _cameraSpawnGirlSide;

	//PLAYERS AND ABILITIES
	[Header ("PLAYERS AND ABILITIES")]
	public GameObject girlObject;
	public GirlBrain girl;
	public GameObject boyObject;
	public BoyBrain boy;
	public bool isChanged = false;
	public Color baseColor = new Color(255, 255, 255);
	public GameObject previousObject;
	public MeshRenderer previousRender;
	public CameraFollow cam;
	public bool checkpointsReached;
	public static bool checkpointStatic;
	public GameObject previousObjectCommand;
	public GameObject selectedCharacterArrow;
	public Color defaultBoxColor;

	//ALARMS
	[Header("ALARMS")]
	public List<GameObject> _alarmsList = new List<GameObject>();
	private GameObject _closestAlarm;
	public GameObject lightsContainer;
	private GameObject _light;
	private Light _alarmLight;
	private bool _lightsOn;
	public float _lightTime;
	public float maxLightTime;
	private bool _sum = true;
	public float maxLightIntensity;
	public float minLightIntensity;
	public float lightDefaultIntensity;
	public float lightsSpeed;
	public bool alarmOn;
	private int _spawnToActivate;
	private Door _doorToUse;
	public AudioClip alarmSound;
	public AudioClip music;
	public AudioClip step1;
	public AudioClip step2;


	//MANAGERS
	[Header("Manager")]
	public AudioManager audioManager;  //necesario que este en escena
	public ScenesManager sceneManager;

	//UI
	[Header("UI")]
	public GameObject girlIcon;
	public GameObject girlIconOff;
	public GameObject boyIcon;
	public GameObject boyIconOff;
	public GameObject powerIcon;
	public GameObject commandIcon;
	public GameObject pauseScreen;
	private bool _paused;
	public UnityEngine.UI.Button menuButton;


	private bool _canCheckKeys;
	private void Awake()
	{
		cam = Camera.main.GetComponent<CameraFollow>();
		checkpointsReached = checkpointStatic;
		sceneManager = FindObjectOfType<ScenesManager>();
	}

	// Start is called before the first frame update
	void Start()
	{
		menuButton.onClick.AddListener(sceneManager.Menu);
		Time.timeScale = 1;
		_canCheckKeys = true;
		girlObject = GameObject.Find("Girl");
		girl = girlObject.GetComponent<GirlBrain>();
		boyObject = GameObject.Find("Boy");
		boy = boyObject.GetComponent<BoyBrain>();
		boy.selected = true;

		audioManager = FindObjectOfType<AudioManager>();
		audioManager.PlayMusic(music);
		cam.characterToFollow = boyObject.transform;
		_enemiesContainer = GameObject.Find("Enemies Container");
		if (_enemiesContainer != null)
		{
			foreach (Transform enemy in _enemiesContainer.transform)
			{
				_enemiesList.Add(enemy.gameObject);
			}
		}

		if (roomsList.Count != 0)
		{
			for (int i = 0; i < roomsList.Count; i++)
			{
				foreach (Transform obj in roomsList[i].transform)
				{
					if (obj.name == "Spawn")
						_spawnsList.Add(obj.gameObject);

					if (obj.name == "Alarm")
						_alarmsList.Add(obj.gameObject);
				}
			}
		}

		selectedCharacterArrow.SetActive(false);
		pauseScreen.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (_canCheckKeys)
			CheckKeys();

		if (_lightsOn)
		{
			LightsTimer();
			LightsFlicker();
		}

		if (controllingEnemy == false && isChanged)
		{
			cam.characterToFollow = girlObject.transform;
		}
		if (checkpointsReached)
		{
			checkpointStatic = true;
		}
	}


	private void CheckKeys()
	{
		if (Input.GetKeyDown(KeyCode.C) && controllingEnemy == false)
			ChangeCharacter();

		if (Input.GetKeyDown(KeyCode.Escape))
			Pause();

	}

	//HACE EL CAMBIO DE CONTROLES DE LA CHICA AL ENEMIGO Y VICEVERSA
	public void ChangePlayerToEnemy(bool activation)
	{
		if (activation)
		{
			switch (_enemyToSelect)
			{
				case 1:
					controllingEnemy = true;
					//girl.selected = false;
					girl.controllingEnemy = true;
					enemyToControl.controlled = true;
					StartCoroutine(ArrowToSelectedCharacter(enemyToControl.gameObject));
					cam.characterToFollow = enemyToControl.gameObject.transform;
					scientificToControl.body.anim.SetBool("Dizzy", true);
					enemyToControl.particle.SetActive(true);
					break;

				case 2:
					controllingEnemy = true;
					//girl.selected = false;
					girl.controllingEnemy = true;
					scientificToControl.controlled = true;
					print("CIENTIFICO: " + scientificToControl.name + "seleccionado");
					StartCoroutine(ArrowToSelectedCharacter(scientificToControl.gameObject));
					cam.characterToFollow = enemyPos;
					scientificToControl.body.anim.SetBool("Dizzy", true);
					scientificToControl.particle.SetActive(true);
					scientificToControl.eCommand.SetActive(false);
					break;

					/* case 3:
						 guardToControl.controlled = true;
						 cam.characterToFollow = guardToControl.transform;
						 print("CONTROLO AL GUARDIA");
						 break;*/
			}
			commandIcon.SetActive(false);
		}

		if (activation == false && controllingEnemy == false)
		{
			switch (_enemyToSelect)
			{
				case 1:

					enemyToControl.controlled = false;
					enemyToControl.particle.SetActive(false);
					girl.selected = true;
					girl.controllingEnemy = false;
					StopCoroutine("ArrowToSelectedCharacter");
					StartCoroutine(ArrowToSelectedCharacter(girlObject));
					break;

				case 2:
					scientificToControl.controlled = false;
					scientificToControl.particle.SetActive(false);
					girl.selected = true;
					girl.controllingEnemy = false;
					StopCoroutine("ArrowToSelectedCharacter");
					StartCoroutine(ArrowToSelectedCharacter(girlObject));
					break;

					/*case 3:
						guardToControl.controlled = false;
						guardToControl._moveToPos = true;
						print("YA NO CONTROLO AL GUARDIAAAA");
						break;*/
			}
			commandIcon.SetActive(true);
		}
	}


	//HACE EL CAMBIO ENTRE LOS 2 PERSONAJES
	private void ChangeCharacter()
	{
		if (isChanged && controllingEnemy == false)
		{
			boy.selected = true;
			girl.selected = false;
			isChanged = false;
			cam.follow = true;
			boyIcon.SetActive(true);
			boyIconOff.SetActive(false);
			girlIcon.SetActive(false);
			girlIconOff.SetActive(true);
			powerIcon.SetActive(false);
			StopCoroutine("ArrowToSelectedCharacter");
			StartCoroutine(ArrowToSelectedCharacter(boyObject));
			cam.characterToFollow = boyObject.transform;
		}
		else if (isChanged == false && controllingEnemy == false)
		{
			girl.selected = true;
			boy.selected = false;
			isChanged = true;
			cam.follow = true;
			girlIcon.SetActive(true);
			girlIconOff.SetActive(false);
			boyIcon.SetActive(false);
			boyIconOff.SetActive(true);
			powerIcon.SetActive(true);
			StopCoroutine("ArrowToSelectedCharacter");
			StartCoroutine(ArrowToSelectedCharacter(girlObject));
			cam.characterToFollow = girlObject.transform;
		}

	}

	IEnumerator ArrowToSelectedCharacter(GameObject controlledCharacter)
	{
		selectedCharacterArrow.transform.position = controlledCharacter.transform.position + new Vector3(0, 3, 0);
		selectedCharacterArrow.SetActive(true);

		yield return new WaitForSecondsRealtime(.5f);
		selectedCharacterArrow.SetActive(false);
	}

	public void EnemyInPlayerFOV(bool watching, GameObject selectedEnemy, string tag)
	{
		if (watching == true)
		{
			print("WATCHING TRUE");
			if (tag == "Patrol")
			{
				print("PATROL");
				if (enemyToControl != null)
					enemyToControl.render.material.color = Color.white; //Si cambio la seleccion del enemigo, va a volver al color "original"
				enemyToControl = selectedEnemy.GetComponent<EnemyBrain>();
				enemyToControl.render.material.color = new Color(0, 0, 255);
				if (girl.selected)
					enemyToControl.eCommand.SetActive(true);
				enemyPos = selectedEnemy.transform;
				_enemyToSelect = 1;
			}

			if (tag == "Scientific")
			{
				print("SCIENTIFIC");
				if (scientificToControl != null)
					scientificToControl.render.material.color = Color.white; //Si cambio la seleccion del enemigo, va a volver al color "original"
				scientificToControl = selectedEnemy.GetComponent<ScientificBrain>();
				if (scientificToControl == null)
					print("CIENTIFICO NULO");
				scientificToControl.render.material.color = new Color(0, 0, 255);
				if(girl.selected)
					scientificToControl.eCommand.SetActive(true);
				enemyPos = selectedEnemy.transform;
				_enemyToSelect = 2;
			}

			/*if (tag == "Guard")
            {
                if (guardToControl != null)
                    guardToControl.render.material.color = Color.white; //Si cambio la seleccion del enemigo, va a volver al color "original"
                guardToControl = selectedEnemy.GetComponent<AlarmGuardBrain>();
                guardToControl.render.material.color = new Color(0, 0, 255);
                _enemyToSelect = 3;
            }     */
		}

		if (watching == false)
		{
			switch (_enemyToSelect)
			{
				case 1:
					if (enemyToControl != null)
					{
						enemyToControl.render.material.color = Color.white;
						enemyToControl = null;
						_enemyToSelect = 0;
					}
					break;

				case 2:
					if (scientificToControl != null)
					{
						scientificToControl.render.material.color = Color.white;
						scientificToControl.eCommand.SetActive(false);
						scientificToControl = null;
						_enemyToSelect = 0;
					}
					break;

					/*case 3:
						guardToControl.render.material.color = Color.white;
						guardToControl = null;
						_enemyToSelect = 0;
						break;*/
			}
		}
		//cam.follow = true;

	}

	public void ObjectInPlayerFOV(bool watching, GameObject selectedObject)
	{

		if (watching == true)
		{
			if (previousObject == null) //Esto solamente se va a hacer cuando la lista de objetos del fov esta vacia
			{
				//Guardo una copia del objeto para tener acceso cuando selecciono uno nuevo
				previousObject = selectedObject;
				previousRender = previousObject.GetComponent<MeshRenderer>();
				MeshRenderer renderer = selectedObject.GetComponent<MeshRenderer>();
				renderer.material.color = Color.red;

				foreach (Transform item in previousObject.transform)
				{
					if (item.gameObject.name == "Boton E")
					{
						previousObjectCommand = item.gameObject; //Copia del icono 
						previousObjectCommand.SetActive(true);
					}
				}
			}

			foreach (Transform item in previousObject.transform)
			{
				if (item.gameObject.name == "Boton E")
				{
					if (item.gameObject != previousObjectCommand)
						previousObjectCommand.SetActive(false);
					previousObjectCommand = item.gameObject;
					previousObjectCommand.SetActive(true);
				}
			}

			if (previousObject != selectedObject && boy.holding == false) //Cuando selecciono un nuevo objeto (con Q/E)
			{
				previousRender.material.color = Color.white; //Restablece el color del objeto anterior
															 //Creo una copia del nuevo objeto como al principio
				previousObject = selectedObject;
				previousRender = previousObject.GetComponent<MeshRenderer>();
				MeshRenderer renderer = selectedObject.GetComponent<MeshRenderer>();
				renderer.material.color = Color.red;
			}


		}


		//Si tengo un objeto guardado y no estoy viendo a ningun otro, lo restablece
		if (previousObject != null && watching == false)
		{
			previousRender.material.color = defaultBoxColor;
			previousRender = null;
			previousObject = null;
			previousObjectCommand.SetActive(false);
			previousObjectCommand = null;
		}

	}

	private void ClosestGuard(Vector3 playerPos)
	{
		for (int i = 0; i < _enemiesList.Count; i++)
		{
			if (i == 0)
				_closestEnemy = _enemiesList[0];

			float distance1 = Vector3.Distance(_closestEnemy.transform.position, playerPos);
			float distance2 = Vector3.Distance(_enemiesList[i].transform.position, playerPos);

			if (distance2 < distance1)
				_closestEnemy = _enemiesList[i];
		}
	}

	public void TellPlayerPosition(Vector3 playerPos, string playerTag)
	{
		if (alarmOn == false)
		{
			ClosestGuard(playerPos);
			var guard = _closestEnemy.GetComponent<EnemyBrain>();
			if (guard != null)
			{
				guard.hasWp = false;
				guard.PlayerSeenPosition(playerPos);
			}
		}

		if (alarmOn && _cameraSpawnBoySide != null && playerTag == "Boy")
		{
			print("llamo al update position");
			_cameraSpawnBoySide.UpdatePosition(playerPos);
		}

		if (alarmOn && _cameraSpawnGirlSide != null && playerTag == "Girl")
		{
			print("llamo al update position");
			_cameraSpawnGirlSide.UpdatePosition(playerPos);
		}
	}

	public Vector3 ClosestAlarm(Transform whoAsks)
	{
		for (int i = 0; i < _alarmsList.Count; i++)
		{
			if (i == 0)
			{
				_closestAlarm = _alarmsList[0];
				_spawnToActivate = 0;
			}


			float distance1 = Vector3.Distance(_closestAlarm.transform.position, whoAsks.position);
			float distance2 = Vector3.Distance(_alarmsList[i].transform.position, whoAsks.position);

			if (distance2 < distance1)
			{
				_closestAlarm = _alarmsList[i];
				_spawnToActivate = i;
			}

		}

		return _closestAlarm.transform.position;
	}

	public void ActivateAlarm(Vector3 playerPos, bool camera, GuardSpawn spawnToUse, Door door, string playerTag)
	{
		if (alarmOn == false)
		{
			if (lightsContainer != null)
				lightsContainer.SetActive(true);
			alarmOn = true;
			_lightsOn = true;
			if (door)
				print("TENGO PUERTAAAA");
			_doorToUse = door.GetComponent<Door>();
			_doorToUse.audioManager.PlaySFX(_doorToUse.slideDoor);
			_doorToUse.aStepped = true;
			_doorToUse.bStepped = true;
			if (camera)
			{
				print("ACTIVATE ALARM CAMERA TRUE");
				SpawnCameraGuards(playerPos, spawnToUse, playerTag);
			}
			else SpawnAlarmGuards(playerPos, _spawnToActivate);

			audioManager.PlaySFX(alarmSound, 0.25f);
		}
		
	}

	private void LightsFlicker()
	{
		if (lightsContainer != null)
			foreach (Transform light in lightsContainer.transform)
			{
				_light = light.gameObject;
				_alarmLight = _light.GetComponent<Light>();

				if (_alarmLight.intensity >= maxLightIntensity)
					_sum = false;
				else if (_alarmLight.intensity <= minLightIntensity)
					_sum = true;
				if (_sum)
					_alarmLight.intensity += Time.deltaTime * lightsSpeed;
				else _alarmLight.intensity -= Time.deltaTime * lightsSpeed;
			}
	}

	private void LightsTimer()
	{
		if (_lightTime <= maxLightTime)
			_lightTime += Time.deltaTime;
		else
		{
			_lightTime = 0;
			_lightsOn = false;
			DeactivateAlarm();
		}
	}

	private void DeactivateAlarm()
	{
		if (lightsContainer != null)
		{
			foreach (Transform light in lightsContainer.transform)
			{
				_light = light.gameObject;
				_alarmLight = _light.GetComponent<Light>();
				_alarmLight.intensity = lightDefaultIntensity;
			}
			lightsContainer.SetActive(false);
		}

		if (_alarmSpawned)
		{
			foreach (var spawnObj in _spawnsList)
			{
				GuardSpawn spawn = spawnObj.GetComponent<GuardSpawn>();
				if (spawn != null)
					spawn.ReturnToSpawn();
			}
			_alarmSpawned = false;
		}

		if (_cameraSpawned)
		{
			_cameraSpawnBoySide.ReturnToSpawn();
			_cameraSpawnBoySide = null;
			_cameraSpawned = false;
		}
		alarmOn = false;

		if (_cameraSpawnGirlSide != null)
			_cameraSpawnGirlSide = null;

		if (_cameraSpawnBoySide != null)
			_cameraSpawnBoySide = null;
	}

	public void CloseDoors()
	{
		_doorToUse.aStepped = false;
		_doorToUse.bStepped = false;
		_doorToUse.canClose = true;
		_doorToUse = null;
		alarmOn = false;
	}

	private void SpawnAlarmGuards(Vector3 seenPlayerPos, int spawnToUse)
	{
		_cameraSpawnBoySide = _spawnsList[spawnToUse].GetComponent<GuardSpawn>();
		_cameraSpawnBoySide.posToMove = seenPlayerPos;
		_cameraSpawnBoySide.SpawnGuards();
		_alarmSpawned = true;
	}

	private void SpawnCameraGuards(Vector3 seenPlayerPos, GuardSpawn spawn, string playerTag)
	{
		if (playerTag == "Boy")
		{
			_cameraSpawnBoySide = spawn.GetComponent<GuardSpawn>();
			_cameraSpawnBoySide.posToMove = seenPlayerPos;
			_cameraSpawnBoySide.SpawnGuards();
		}

		if (playerTag == "Girl")
		{
			_cameraSpawnGirlSide = spawn.GetComponent<GuardSpawn>();
			_cameraSpawnGirlSide.posToMove = seenPlayerPos;
			_cameraSpawnGirlSide.SpawnGuards();
		}
		_cameraSpawned = true;
	}

	public void Pause()
	{
		if (_paused == false)
		{
			pauseScreen.SetActive(true);
			Time.timeScale = 0;
			_paused = true;
			_canCheckKeys = false;
			print("ESTOY EN PAUSA");
		}
		else
		{
			pauseScreen.SetActive(false);
			Time.timeScale = 1;
			_paused = false;
			_canCheckKeys = true;
			print("APRETA PAUSA");
		}
	}
}
