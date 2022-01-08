using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmComputer : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private Door _door;
    
    public delegate void AlarmActivation(Vector3 playerPos, Vector3 spawnPoint, Door door);

    public event AlarmActivation OnAlarmActivation;
    
    public void TriggerAlarm(Vector3 playerPos)
    {
        if (_door)
            _door.OpenDoor();

        OnAlarmActivation?.Invoke(playerPos, _spawnPoint.position, _door);
        
    }
}
