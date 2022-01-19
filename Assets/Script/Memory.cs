using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Memory : MonoBehaviour
{
    public static Memory Instance;

    public string activeLevel;

    public bool checkpointReached;
    
    [SerializeField] private Vector3 _boyPosition;
    [SerializeField] private Vector3 _girlPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void SavePosition()
    {
        Debug.Log("save");
        _boyPosition = FindObjectOfType<Boy>().transform.position;
        _girlPosition = FindObjectOfType<Girl>().transform.position;

        checkpointReached = true;
    }
    
    public Vector3 GetBoyPosition()
    {
        return _boyPosition;
    }

    public Vector3 GetGirlPosition()
    {
        return _girlPosition;
    }
}
