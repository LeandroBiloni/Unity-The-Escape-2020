using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    public float speed;
    public ScenesManager scenes;
    [HideInInspector] public SpikeBallSpawn mySpawner;
    private void Start()
    {
        scenes = FindObjectOfType<ScenesManager>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Destroy balls"))
            mySpawner.ReturnBall(this);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            scenes.LoseScreen();
    }

    public static void TurnOn(SpikeBall spikeBall)
	{
        spikeBall.gameObject.SetActive(true);
	}

    public static void Turnoff(SpikeBall spikeBall)
	{
        spikeBall.gameObject.SetActive(false);
        
    }
}
