using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
	//PLAYERS AND ABILITIES
	[Header ("PLAYERS AND ABILITIES")]
	public bool checkpointsReached;
	public static bool checkpointStatic;

	//ALARMS
	[Header("ALARMS")]
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
	public GameObject pauseScreen;
	private bool _paused;
	public UnityEngine.UI.Button menuButton;


	private bool _canCheckKeys;
	private void Awake()
	{
		checkpointsReached = checkpointStatic;
		sceneManager = FindObjectOfType<ScenesManager>();
	}

	// Start is called before the first frame update
	void Start()
	{
		menuButton.onClick.AddListener(sceneManager.Menu);
		Time.timeScale = 1;
		_canCheckKeys = true;

		audioManager = FindObjectOfType<AudioManager>();
		audioManager.PlayMusic(music);

		pauseScreen.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (_canCheckKeys)
			CheckKeys();
		
		if (checkpointsReached)
		{
			checkpointStatic = true;
		}
	}


	private void CheckKeys()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Pause();

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
