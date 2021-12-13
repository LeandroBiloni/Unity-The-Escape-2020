using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	private static CheckpointManager instance;
	public Vector3 boyCheckpointPos;
	public Vector3 girlCheckpointPos;
	public bool active1;
	public bool active2;
	public Transform boy;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			
			Destroy(gameObject);
		}

	}

	/*private void Update()
	{
		if (active2)
		{
			boyCheckpointPos = boy.position;
		}
	}
	*/
}
