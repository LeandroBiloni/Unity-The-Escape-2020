using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AlarmsManager : MonoBehaviour
{
    public static AlarmsManager Instance;
    [SerializeField] private AlarmGuard _guard;
    [SerializeField] private float _alarmDuration;
    [SerializeField] private List<Light> _lights = new List<Light>();
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private float _minLightIntensity;
    [SerializeField] private float _lightDefaultIntensity;
    [SerializeField] private float _lightOscilationSpeed;
    [SerializeField] private Color _lightDefaultColor;
    [SerializeField] private Color _lightAlarmColor;
    [SerializeField] private AudioClip _alarmSound;
    private bool _increaseIntensity;
    private bool _alarmActive;

    private int _spawnedGuards;
    
    private AudioManager _audioManager;
    public delegate void Activation();

    public event Activation OnDeactivation;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _audioManager = FindObjectOfType<AudioManager>();
        var alarms = FindObjectsOfType<AlarmComputer>();

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
        _alarmActive = true;
        SpawnGuard(playerPos, spawnPoint, door);
        foreach (var l in _lights)
        {
            l.color = _lightAlarmColor;
        }
        StartCoroutine(LightsFlicker());
        _audioManager.PlaySFX(_alarmSound, 0.25f);
    }

    public void AlarmOff()
    {
        _alarmActive = false;
        _audioManager.StopSFX(_alarmSound);
        foreach (var l in _lights)
        {
            l.intensity = _lightDefaultIntensity;
            l.color = _lightDefaultColor;
        }
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
        _spawnedGuards++;
        var guard = Instantiate(_guard, spawnPoint, Quaternion.identity);
        guard.SetSpawnPosition(spawnPoint);
        guard.SetPlayerSeenPosition(playerPos);

        if (door) guard.OnReturn += () =>
        {
            if (_spawnedGuards <= 1)
            {
                door.CloseDoor();
            }
            _spawnedGuards--;
        };

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

        AlarmOff();
        
        action?.Invoke();
        
        OnDeactivation?.Invoke();

        OnDeactivation = null;
    }
    public bool IsAlarmActive()
    {
        return _alarmActive;
    }

    private void OnDisable()
    {
        Destroy(Instance.gameObject);
    }
}
