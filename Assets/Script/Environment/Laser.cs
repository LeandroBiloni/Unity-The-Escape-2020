using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private ScenesManager _scenesManager;
    // Start is called before the first frame update
    void Start()
    {
        _scenesManager = FindObjectOfType<ScenesManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            _scenesManager.LoseScreen();
    }
}
