using System;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool _active = false;

    [SerializeField] private List<Door> _doorsList;
    //public Transform[] myCables;
    [SerializeField] private List<Cable> _cables = new List<Cable>();
    [SerializeField] private GameObject _commandIcon;
    private Boy _boy;
    [SerializeField] private float _keyIconActivationDistance;
    private MeshRenderer _meshRenderer;
    private Color _defaultColor;
    public delegate void Activation();

    public event Activation OnActivation;
    public event Activation OnDeactivation;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _boy = FindObjectOfType<Boy>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _defaultColor = _meshRenderer.material.color;
        
        if (_doorsList.Count > 0)
        {
            foreach (var door in _doorsList)
            {
                door.AddButton(this);
            }
        }
    }

    private void Update()
    {
        if (!_meshRenderer.isVisible) return;

        if (_boy.IsHoldingObject())
        {
            if (Vector3.Distance(_boy.transform.position, transform.position) <= _keyIconActivationDistance)
            {
                _commandIcon.SetActive(true);
            }
        }
        else _commandIcon.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects") || (other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            _active = true;       
            _meshRenderer.material.color = Color.green;
            OnActivation?.Invoke();
            if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
            {
                other.gameObject.transform.position = transform.position;
            }
            CablesOn();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects") || (other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            _active = false;     
            _meshRenderer.material.color = _defaultColor;
            CablesOff();
            OnDeactivation?.Invoke();
        }
        
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

    public bool IsActive()
    {
        return _active;
    }
}
