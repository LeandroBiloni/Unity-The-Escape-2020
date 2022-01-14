using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _moveDistance;
    [SerializeField] private bool _onlyOpen;
    private bool _isOpen;

    private HashSet<Button> _buttons = new HashSet<Button>();
    private HashSet<Switches> _switches = new HashSet<Switches>();

    private int _buttonsCount;
    [SerializeField] private int _switchesCount;
    
    [SerializeField] private Light _doorLight;
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private Color _activeColor;
    public AudioManager audioManager;
    public AudioClip slideDoor;
    public AudioClip disconectDoor;
    [SerializeField] private List<Cable> _cables = new List<Cable>();
    private Vector3 _startPos;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        _startPos = transform.position;
    }

    public void OpenDoor(Action action = null)
    {
        StartCoroutine(Open(action));
        CablesOn();
        
        if (_doorLight)
            _doorLight.color = _activeColor;
        
        audioManager.PlaySFX(slideDoor, 1f);
    }

    IEnumerator Open(Action action = null)
    {
        if (!_isOpen)
        {
            while (transform.position.y > _startPos.y - _moveDistance)
            {
                transform.position -= transform.up * (_speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            _isOpen = true;
        
            action?.Invoke();
        }
    }

    IEnumerator Close()
    {
        if (!_onlyOpen && _isOpen)
        {
            while (transform.position.y < _startPos.y)
            {
                transform.position += transform.up * (_speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            _isOpen = false; 
        }
    }

    public void CloseDoor()
    {
        StartCoroutine(Close());
        CablesOff();
        
        if (_doorLight)
                _doorLight.color = _inactiveColor;
    }
    
    

    private void CablesOn()
    {
        foreach (var cable in _cables)
        {
            cable.Activate();
        }
    }

    private void CablesOff()
    {
        foreach (var cable in _cables)
        {
            cable.Deactivate();
        }
    }

    public void AddButton(Button button)
    {
        _buttons.Add(button);
        button.OnActivation += ActiveButton;
        button.OnDeactivation += InactiveButton;
    }

    public void AddSwitch(Switches @switch)
    {
        _switches.Add(@switch);
        @switch.OnActivation += ActiveSwitch;
        @switch.OnDeactivation += InactiveSwitch;
    }

    private void CheckButtons()
    {
        if (_buttonsCount == _buttons.Count && !_isOpen)
            OpenDoor();
        else if (!_onlyOpen && _isOpen) CloseDoor();
    }

    private void CheckSwitches()
    {
        if (_switchesCount == _switches.Count)
        {
            foreach (var sw in _switches)
            {
                if (!sw.IsActive())
                {
                    Debug.Log("return");
                    return;
                }
            }
            OpenDoor();
            foreach (var s in _switches)
            {
                s.DeactivateInteraction();
            }
        }
    }

    private void ActiveButton()
    {
        _buttonsCount++;
        CheckButtons();
    }

    private void InactiveButton()
    {
        _buttonsCount--;
        if (_buttonsCount < 0)
            _buttonsCount = 0;
        CheckButtons();
    }

    private void ActiveSwitch()
    {
        _switchesCount++;
        CheckSwitches();
    }

    private void InactiveSwitch()
    {
        _switchesCount--;
        if (_switchesCount < 0)
            _switchesCount = 0;
    }

    public bool IsOpen()
    {
        return _isOpen;
    }
}
