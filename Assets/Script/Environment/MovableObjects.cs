using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    private Transform trans;

    private void Start()
    {
        trans = GetComponent<Transform>();
    }
    void Update()
    {
        if (trans.localEulerAngles != Vector3.zero)
            trans.localEulerAngles = Vector3.zero;
    }
}
