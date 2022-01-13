﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPlatforms : MonoBehaviour
{
    [SerializeField] private bool _platUp;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxTime;
    private float _time;

    // Update is called once per frame
    void Update()
    {
        Timer();
        Move();
    }

    private void Timer()
    {
        _time += Time.deltaTime;

        if (_time >= _maxTime)
        {
            _platUp = !_platUp;
            _time = 0;
        }
    }

    private void Move()
    {
        if (_platUp)
            transform.position += transform.up * _speed * Time.deltaTime;
        else transform.position -= transform.up * _speed * Time.deltaTime;
    }
}
