using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallSpawn : MonoBehaviour
{
    private float _time;
    public float maxTime;
    public SpikeBall ballPrefab;
    private bool _spawn;
    // Start is called before the first frame update
    void Start()
    {
        _spawn = false;
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        Timer();

        if (_spawn)
            SpawnBall();
    }

    private void Timer()
    {
        _time += Time.deltaTime;

        if (_time >= maxTime)
        {
            _spawn = true;
            _time = 0;
        }
    }

    private void SpawnBall()
    {
        SpikeBall ball = Instantiate(ballPrefab, transform);
        ball.transform.localPosition = Vector3.zero;
        _spawn = false;
    }
}
