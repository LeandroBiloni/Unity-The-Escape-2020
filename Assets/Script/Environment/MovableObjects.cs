using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    private Transform trans;

    [SerializeField] private GameObject _interactionIcon;

    private Color _originalColor;
    private void Start()
    {
        trans = GetComponent<Transform>();
        _originalColor = GetComponent<MeshRenderer>().material.color;
    }
    void Update()
    {
        if (trans.localEulerAngles != Vector3.zero)
            trans.localEulerAngles = Vector3.zero;
    }

    public void Selected()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        _interactionIcon.SetActive(true);
    }
    
    public void Deselected()
    {
        GetComponent<MeshRenderer>().material.color = _originalColor;
        _interactionIcon.SetActive(false);
    }
}
