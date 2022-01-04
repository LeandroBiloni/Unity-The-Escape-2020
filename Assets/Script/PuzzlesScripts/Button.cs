using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool _active = false;
    //public Transform[] myCables;
    private Cable[] _cables;
    [SerializeField] private GameObject _commandIcon;
    private Boy _boy;
    [SerializeField] private float _keyIconActivationDistance;
    private MeshRenderer _meshRenderer;

    public delegate void Activation();

    public event Activation OnActivation;
    private void Start()
    {
        _boy = FindObjectOfType<Boy>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        var childs = transform.GetComponentsInChildren<Cable>();
        
        _cables = childs;
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
            OnActivation?.Invoke();
            if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
            {
                other.gameObject.transform.position = transform.position;
                //other.attachedRigidbody.isKinematic = true;
            }
            CablesOn();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects") || (other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            _active = false;      
            // if(other.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
            // {
            //     other.attachedRigidbody.isKinematic = false;
            // }
            CablesOff();
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
