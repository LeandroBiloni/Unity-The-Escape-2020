using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool _move;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject limits;
    private bool _interactable;

    private void Start()
    {
        _interactable = true;
    }

    public void StartMovement()
    {
        if (!_interactable)
        {
            Debug.Log("no interactable");
            return;
        }
        
        Debug.Log("start movement");
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (transform.localPosition.y < _maxHeight)
        {
            transform.localPosition += transform.up * (_speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        limits.SetActive(false);
        _interactable = false;
    }

    public bool IsInteractable()
    {
        return _interactable;
    }
}
