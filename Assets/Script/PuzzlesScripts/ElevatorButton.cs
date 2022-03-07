using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    [SerializeField] private bool _active;
    [SerializeField] private Elevator _elevator;

    // Start is called before the first frame update
    void Start()
    {
        _elevator.AddButton(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _active = true;
            _elevator.CheckButtons();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _active = false;
    }

    public bool IsActive()
    {
        return _active;
    }
}
