using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Character
{
    private Color _originalColor;
    // Start is called before the first frame update
    void Start()
    {
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
