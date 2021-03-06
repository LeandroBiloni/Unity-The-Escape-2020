using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(EnablingDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Memory.Instance.SavePosition();
            Debug.Log("trigger");
        }
        
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    IEnumerator EnablingDelay()
    {
        var collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        yield return new WaitForSeconds(1);

        collider.enabled = true;
    }
}
