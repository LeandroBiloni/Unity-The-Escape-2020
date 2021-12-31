using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : Character
{
    private Color _originalColor;

    protected NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _originalColor = GetComponent<MeshRenderer>().material.color;
    }

    public void UnitInPlayerFOV()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void UnitOutOfPlayerFOV()
    {
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    public override void Select()
    {
        base.Select();
        _canMove = true;
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }

    public override void Deselect()
    {
        base.Deselect();
        GetComponent<MeshRenderer>().material.color = _originalColor;
    }
}
