﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _rotationSpeed;
    [SerializeField] protected bool _canMove;
    
    [SerializeField] protected bool _selected;

    protected Animator _animator;

    [SerializeField] protected GameObject _selectionIcon;

    [SerializeField] protected float _iconShowTime;
    
    protected FieldOfView _fieldOfView;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _fieldOfView = GetComponent<FieldOfView>();
        _selectionIcon.SetActive(false);
        _animator = GetComponent<Animator>();
        _canMove = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_selected) return;

        if (_canMove)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            if (x != 0 || z != 0)
            {
                Vector3 dir = Vector3.zero;
                dir = new Vector3(x, 0, z);

                ManualMovement(dir);
            }
        }
    }

    /// <summary>
    /// Movement for selected characters.
    /// </summary>
    protected void ManualMovement(Vector3 dir)
    {
        if (_animator)
        {
            _animator.SetFloat("VelZ", dir.z);
            _animator.SetFloat("VelX", dir.x);  
        }
        
        transform.LookAt(transform.position + dir);
        transform.position += dir.normalized * (_moveSpeed * Time.deltaTime);
        
        //TODO: Agregar sonido.
        //audMan.WalkingSound(dir);
    }

    public bool IsSelected()
    {
        return _selected;
    }

    /// <summary>
    /// Actions for the selected character.
    /// </summary>
    public virtual void Select()
    {
        _selected = true;
        StartCoroutine(ActivateSelectionIconWithTimer());
    }

    /// <summary>
    /// Actions for the deselected character.
    /// </summary>
    public virtual void Deselect()
    {
        _selected = false;
    }

    /// <summary>
    /// Activates the Selection Icon.
    /// </summary>
    protected IEnumerator ActivateSelectionIconWithTimer()
    {
        _selectionIcon.SetActive(true);

        yield return new WaitForSeconds(_iconShowTime);
        
        _selectionIcon.SetActive(false);
    }
}
