using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallSpawn : MonoBehaviour
{
    public float maxTime;
    public SpikeBall ballPrefab;
    public int stock = 10;

    public ObjectPool<SpikeBall> pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<SpikeBall>(SpikeBallFactory, SpikeBall.TurnOn, SpikeBall.Turnoff, stock, true);
        StartCoroutine(SpawnSpikeBall());
    }

    public SpikeBall SpikeBallFactory()
	{
        var tempBall = Instantiate(ballPrefab, transform);
        tempBall.transform.localPosition = Vector3.zero;
        tempBall.mySpawner = this;
        return tempBall;
	}

    public void ReturnBall(SpikeBall spikeBall)
	{
        pool.ReturnObject(spikeBall);
	}

    IEnumerator SpawnSpikeBall()
	{
        yield return new WaitForSeconds(maxTime);
        pool.GetObject();
        StartCoroutine(SpawnSpikeBall());
	}
}
