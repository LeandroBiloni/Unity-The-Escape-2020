using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOpen : MonoBehaviour
{

    public Door door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door)
        {
            door.audioManager.PlaySFX(door.slideDoor, 1.7f);
        }
    }
}
