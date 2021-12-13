using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool move = false;
    public float speed;
    public float _time;
    public float maxTime;
    public GameObject limits;
    public bool activated;

    // Update is called once per frame
    void Update()
    {
        if (move == true && activated == false)
            Move();
    }

    public void Move()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        _time += Time.deltaTime;

        if (_time >= maxTime)
        {
            limits.SetActive(false);
            move = false;
            activated = true;
        }
    }
}
