using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDoor : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Platform _platform;
    [SerializeField] private GameObject _activationKeyIcon;
    [SerializeField] private KeyCode _interactionKey;
    public Cable[] _cables;

    private void Start()
	{
        var childs = transform.GetComponentsInChildren<Cable>();
        
        _cables = childs;
	}
    
    private void OnTriggerStay(Collider other)
    {
        var scientific = other.gameObject.GetComponent<Scientific>();

        if (scientific)
        {
            if (Input.GetKeyDown(_interactionKey))
            {
                Activate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _activationKeyIcon.SetActive(false);
    }

    private void Activate()
    {
        if (_door && !_door.IsOpen())
        {
            _door.OpenDoor();
        }

        if (_platform && _platform.IsInteractable())
        {
            _platform.StartMovement();
        } 
        
        CablesOn();
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
}     
    