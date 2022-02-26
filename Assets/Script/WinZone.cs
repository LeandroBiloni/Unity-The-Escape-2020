
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private ScenesManager _scenesManager;

    private void Start()
    {
        _scenesManager = FindObjectOfType<ScenesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Memory.Instance.OnSceneChange("Win");
            _scenesManager.WinScreen();
        }
    }
}
