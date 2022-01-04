using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmsManager : MonoBehaviour
{
    [SerializeField] private AlarmGuard _guard;
    [SerializeField] private float _alarmDuration;
    [SerializeField] private List<Light> _lights = new List<Light>();
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private float _minLightIntensity;
    [SerializeField] private float _lightOscilationSpeed;
    private bool _increaseIntensity;
    private bool _alarmActive;
    
    public delegate void Activation();

    public event Activation OnDeactivation;
    // Start is called before the first frame update
    void Start()
    {
        var alarms = FindObjectsOfType<Alarm>();

        foreach (var alarm in alarms)
        {
            alarm.OnAlarmActivation += AlarmOn;
        }

        var cameras = FindObjectsOfType<SurveillanceCamera>();
        
        foreach (var camera in cameras)
        {
            camera.OnAlarmActivation += AlarmOn;
        }
    }

    public void AlarmOn(Vector3 playerPos, Vector3 spawnPoint, Door door)
    {
        Debug.Log("effects on");
        _alarmActive = true;
        SpawnGuard(playerPos, spawnPoint, door);
        StartCoroutine(LightsFlicker());
        //TODO: Sonidos
    }

    public void AlarmOff()
    {
        Debug.Log("effects off");
        _alarmActive = false;
    }
    
    IEnumerator LightsFlicker()
    {
        
        while (_alarmActive)
        {
            foreach (var l in _lights)
            {
                if (l.intensity >= _maxLightIntensity)
                    _increaseIntensity = false;
                else if (l.intensity <= _minLightIntensity)
                    _increaseIntensity = true;
                
                if (_increaseIntensity)
                    l.intensity += Time.deltaTime * _lightOscilationSpeed;
                else l.intensity -= Time.deltaTime * _lightOscilationSpeed;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    private void SpawnGuard(Vector3 playerPos, Vector3 spawnPoint, Door door)
    {
        var guard = Instantiate(_guard, spawnPoint, Quaternion.identity);
        guard.SetSpawnPosition(spawnPoint);
        guard.SetPlayerSeenPosition(playerPos);

        if (door) guard.OnReturn += door.CloseDoor;

        guard.OnReturn += () => { Debug.Log("door close"); };

        StartCoroutine(AlarmTimer(guard.ReturnToSpawn));
    }

    IEnumerator AlarmTimer(Action action = null)
    {
        var time = 0f;

        while (time <= _alarmDuration)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _alarmActive = false;
        
        action?.Invoke();
        
        OnDeactivation?.Invoke();

        OnDeactivation = null;
    }

    public bool IsAlarmActive()
    {
        return _alarmActive;
    }
}
