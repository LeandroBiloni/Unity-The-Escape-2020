using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxMoveTime;
    [SerializeField] private bool _onlyOpen;
    private bool _isOpen;

    private HashSet<Button> _buttons = new HashSet<Button>();
    private HashSet<Switches> _switches = new HashSet<Switches>();
    
    [SerializeField] private Light _doorLight;
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private Color _activeColor;
    public AudioManager audioManager;
    public AudioClip slideDoor;
    public AudioClip disconectDoor;
    [SerializeField] private List<Cable> _cables = new List<Cable>();

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        foreach (var sw in _switches)
        {
            sw.OnActivation += CheckSwitches;
        }

        foreach (var button in _buttons)
        {
            button.OnActivation += CheckButtons;
        }
        //myCables = new Transform[cableContainer.childCount];
        //for (int i = 0; i < myCables.Length; i++)
        //{
        //    myCables[i] = cableContainer.GetChild(i);
        //    var myCable = myCables[i].gameObject.GetComponent<Cable>();
        //    cables.Add(myCable);
        //}
    }

    private void Update()
    {
        // if (_onlyOpen)
        // {
        //     // if (openThroughComputer)
        //     // {
        //     //     OpenDoor();
        //     //     if (_doorLight != null)
        //     //         _doorLight.color = Color.green;
        //     //     for (int i = 0; i < cables.Count; i++)
        //     //     {
        //     //         cables[i].activated = true;
        //     //     }
        //     //
        //     // }
        //     //
        //     // if (openThroughSwitches)
        //     // {
        //     //     if (activeSwitches == _switches.Count)
        //     //     {
        //     //         OpenDoor();
        //     //     if (_doorLight != null)
        //     //         _doorLight.color = Color.green;
        //     //     }
        //     // }
        //
        // }

        // if (cameraDoor)
        // {
        //     if (aStepped && bStepped && _stop == false)
        //     {
        //         OpenDoor();
        //         if (_doorLight != null)
        //             _doorLight.color = Color.green;
        //         
        //     }
        //     else if (canClose)
        //     {
        //         CloseDoor();
        //         if (_doorLight != null)
        //             _doorLight.color = Color.red;
        //     }
        // }


        // if (buttonA != null && buttonB != null)
        // {
        //
        //     // if (buttonA._active == true)
        //     //     aStepped = true;
        //     // else aStepped = false;
        //     //
        //     // if (buttonB._active == true)
        //     //     bStepped = true;
        //     // else bStepped = false;
        //
        //     if (aStepped && bStepped)
        //     {
        //         if (_doorLight != null)
        //             _doorLight.color = Color.green;
        //         //canClose = false;
        //         OpenDoor();
        //         // if (_alreadyMoved == false)
        //         //     _alreadyMoved = true;
        //         if (activated == false)
        //         {
        //             print("funcion de sonido");
        //             OpenSound();
        //         }
        //
        //     }
        //     // else if ((!aStepped || !bStepped) && canClose == false && _alreadyMoved)
        //     //     canClose = true;
        //
        //     // if (canClose && _alreadyMoved)
        //     // {
        //     //     CloseDoor();
        //     //
        //     // }
        // }
        //
        // if (aStepped == true && bStepped == true)
        // {
        //     activated = true;
        // }
        //
        // if (aStepped == true && bStepped == false || aStepped ==false && aStepped == true || aStepped == false && bStepped == false)
        // {
        //     activated = false;
        // }
    }

    public void OpenDoor(Action action = null)
    {
        StartCoroutine(MoveDoor(action));
        CablesOn();
        
        if (_doorLight)
            _doorLight.color = _activeColor;
        
        audioManager.PlaySFX(slideDoor, 1f);
    }

    IEnumerator MoveDoor(Action action = null)
    {
        
        var time = 0f;

        while (time <= _maxMoveTime)
        {
            if (!_isOpen)
            {
                transform.position -= transform.up * (_speed * Time.deltaTime);
            }
            else
            {
                if (_onlyOpen) yield break;
                transform.position += transform.up * (_speed * Time.deltaTime);
                
            }
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _isOpen = !_isOpen;
        
        action?.Invoke();
    }

    public void CloseDoor()
    {
        StartCoroutine(MoveDoor());
        CablesOff();
        _doorLight.color = _inactiveColor;
    }
    
    

    private void CablesOn()
    {
        foreach (var cable in _cables)
        {
            // var render = cable.gameObject.GetComponent<Renderer>();
            // Material[] materials = render.materials;
            // materials[0].color = Color.green;
            cable.Activate();
        }
    }

    private void CablesOff()
    {
        foreach (var cable in _cables)
        {
            // var render = cable.gameObject.GetComponent<Renderer>();
            // Material[] materials = render.materials;
            // materials[0].color = cablesDefaultColor;
            cable.Deactivate();
        }
    }

    public void AddButton(Button button)
    {
        _buttons.Add(button);
    }

    public void AddSwitch(Switches switches)
    {
        _switches.Add(switches);
    }

    public void CheckButtons()
    {
        var count = 0;
        foreach (var button in _buttons)
        {
            if (!button.IsActive()) return;

            count++;
            
            if (count == _buttons.Count)
                OpenDoor();
        }
    }

    public void CheckSwitches()
    {
        var count = 0;
        foreach (var sw in _switches)
        {
            if (!sw.IsActive()) return;

            count++;
            
            if (count == _switches.Count)
                OpenDoor();
        }
    }

    public bool IsOpen()
    {
        return _isOpen;
    }
}
