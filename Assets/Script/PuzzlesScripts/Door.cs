using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    public bool onlyOpen;
    public bool cameraDoor;
    private bool _stop;
    public float speed;
    public float _time;
    public float maxTime;
    public bool canClose = false;
    private bool _alreadyMoved = false;
    public Button buttonA;
    public bool aStepped;
    public bool bStepped;
    public Button buttonB;
    public bool openThroughComputer = false;
    public Light doorLight;
    public List<Switches> switches = new List<Switches>();
    public bool openThroughSwitches;
    public float activeSwitches;
    public AudioManager audioManager;
    public AudioClip slideDoor;
    public AudioClip disconectDoor;
    public bool activated;
    public Transform cableContainer;
    public List<Cable> cables = new List<Cable>();
    public Color cablesDefaultColor;
    private bool _cablesOn;


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        activated = false;

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
        if (onlyOpen)
        {
            if (openThroughComputer)
            {
                Open();
                if (doorLight != null)
                    doorLight.color = Color.green;
                for (int i = 0; i < cables.Count; i++)
                {
                    cables[i].activated = true;
                }

            }

            if (openThroughSwitches)
            {
                if (activeSwitches == switches.Count)
                {
                    Open();
                if (doorLight != null)
                    doorLight.color = Color.green;
                }
            }

        }

        if (cameraDoor)
        {
            if (aStepped && bStepped && _stop == false)
            {
                Open();
                if (doorLight != null)
                    doorLight.color = Color.green;
                
            }
            else if (canClose)
            {
                Close();
                if (doorLight != null)
                    doorLight.color = Color.red;
            }
        }


        if (buttonA != null && buttonB != null)
        {

            if (buttonA.stepped == true)
                aStepped = true;
            else aStepped = false;

            if (buttonB.stepped == true)
                bStepped = true;
            else bStepped = false;

            if (aStepped && bStepped)
            {
                if (doorLight != null)
                    doorLight.color = Color.green;
                canClose = false;
                Open();
                if (_alreadyMoved == false)
                    _alreadyMoved = true;
                if (activated == false)
                {
                    print("funcion de sonido");
                    OpenSound();
                }

            }
            else if ((!aStepped || !bStepped) && canClose == false && _alreadyMoved)
                canClose = true;

            if (canClose && _alreadyMoved)
            {
                Close();

            }
        }

        if (aStepped == true && bStepped == true)
        {
            activated = true;
        }

        if (aStepped == true && bStepped == false || aStepped ==false && aStepped == true || aStepped == false && bStepped == false)
        {
            activated = false;
        }
    }

    public void Open()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
        _time += Time.deltaTime;

        if (_time >= maxTime)
        {
            _stop = true;
            if (!cameraDoor)
                canClose = true;
            if (openThroughComputer || openThroughSwitches)
                gameObject.SetActive(false);
        }
        if (_cablesOn == false)
        {
            CablesOn();
            _cablesOn = true;
        }
    }


    public void OpenSound()
    {   
        audioManager.PlaySFX(slideDoor, 1f);
        print("reproduzco el sonido");
        activated = true;
    }

    private void Close()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        _time -= Time.deltaTime;

        if (_time <= 0)
        {
            canClose = false;
            _stop = false;
            _alreadyMoved = false;
            if (doorLight != null)
                doorLight.color = Color.red;
        }

        if (_cablesOn && cables.Count != 0)
        {
            CablesOff();
            _cablesOn = false;
        }
    }

    private void CablesOn()
    {
        foreach (var cable in cables)
        {
            var render = cable.gameObject.GetComponent<Renderer>();
            Material[] materials = render.materials;
            materials[0].color = Color.green;
        }
    }

    private void CablesOff()
    {
        foreach (var cable in cables)
        {
            var render = cable.gameObject.GetComponent<Renderer>();
            Material[] materials = render.materials;
            materials[0].color = cablesDefaultColor;
        }
    }
}
