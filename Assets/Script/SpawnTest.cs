using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public AlarmGuard guard;

    public Transform spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var g = Instantiate(guard, spawn.position, Quaternion.identity);
            g.SetSpawnPosition(spawn.position);
            g.SetPlayerSeenPosition(FindObjectOfType<Girl>().transform.position);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            var guards = FindObjectsOfType<AlarmGuard>();

            foreach (var g in guards)
            {
                g.ReturnToSpawn();
            }
        }
    }
}
