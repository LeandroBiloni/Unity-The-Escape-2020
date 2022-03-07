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

    private Dictionary<int, bool> _doorsStatus = new Dictionary<int, bool>();
    
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
        
        var doors = FindObjectsOfType<Door>();

        _doorsStatus.Clear();
        
        foreach (var door in doors)
        {
            _doorsStatus.Add(door.GetDoorNumber(), door.IsOpen());
        }
    }
    
    public Vector3 GetBoyPosition()
    {
        return _boyPosition;
    }

    public Vector3 GetGirlPosition()
    {
        return _girlPosition;
    }

    public void LoadDoors()
    {
        StartCoroutine(LoadDoorsDelay());
    }

    IEnumerator LoadDoorsDelay()
    {
        yield return new WaitForEndOfFrame();
        
        var newDoors = FindObjectsOfType<Door>();
        foreach (var door in newDoors)
        {
            var doorNumber = door.GetDoorNumber();
            door.SetOpenOrClosedStatus(_doorsStatus[doorNumber]);
        }
    }

    public void OnSceneChange(string nextLevel)
    {
        activeLevel = nextLevel;
        checkpointReached = false;
        _boyPosition = Vector3.zero;
        _girlPosition = Vector3.zero;
        _doorsStatus.Clear();
    }
    
    public void ResetMemory()
    {
        activeLevel = "";
        checkpointReached = false;
        _boyPosition = Vector3.zero;
        _girlPosition = Vector3.zero;
        _doorsStatus.Clear();
    }

}
