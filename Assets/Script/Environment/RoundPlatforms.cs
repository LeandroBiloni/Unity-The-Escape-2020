using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPlatforms : MonoBehaviour
{
    public bool platUp;
    public float speed;
    public float maxTime;
    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        Move();
    }

    private void Timer()
    {
        _time += Time.deltaTime;

        if (_time >= maxTime)
        {
            platUp = !platUp;
            _time = 0;
        }
    }

    private void Move()
    {
        if (platUp)
            transform.position += transform.up * speed * Time.deltaTime;
        else transform.position -= transform.up * speed * Time.deltaTime;
    }
}
