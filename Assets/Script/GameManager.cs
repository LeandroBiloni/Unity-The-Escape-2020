using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
	public AudioClip music;


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

		if (Memory.Instance.checkpointReached)
		{
			var girl = FindObjectOfType<Girl>();
			girl.transform.position = Memory.Instance.GetGirlPosition();

			var boy = FindObjectOfType<Boy>();
			boy.transform.position = Memory.Instance.GetBoyPosition();
			
			Memory.Instance.LoadDoors();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (_canCheckKeys)
			CheckKeys();
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
