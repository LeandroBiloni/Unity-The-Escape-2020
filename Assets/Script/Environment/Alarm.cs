using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    private GameManager _manager;
    public Vector3 playerPos;
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
       _manager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scientific")) 
        {
            ScientificBrain sci = other.GetComponent<ScientificBrain>();
            playerPos = sci.playerPos;
            _manager.ActivateAlarm(playerPos, false, null, door, "");             
        }
    }
}
