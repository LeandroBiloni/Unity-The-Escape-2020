using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    public float speed;
    public ScenesManager scenes;
    private void Start()
    {
        scenes = FindObjectOfType<ScenesManager>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Destroy balls"))
            Destroy(gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            scenes.LoseScreen();
    }
}
