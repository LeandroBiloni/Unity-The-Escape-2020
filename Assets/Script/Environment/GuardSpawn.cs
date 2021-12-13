using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawn : MonoBehaviour
{
    public GameObject guard;
    private GameObject _spawnedGuard;
    public AlarmGuardBrain _brain;
    public Vector3 posToMove;

    public void SpawnGuards()
    {
        _spawnedGuard = Instantiate(guard, gameObject.transform);
        _spawnedGuard.transform.localPosition = Vector3.zero;
        _brain = _spawnedGuard.GetComponent<AlarmGuardBrain>();
        _brain.spawnPoint = transform.position;
        _brain.spawnPoint.y = _spawnedGuard.transform.position.y;
        _brain.posToMove = new Vector3(posToMove.x,_spawnedGuard.transform.position.y,posToMove.z);
    }

    public void ReturnToSpawn()
    {
        _brain.Return();
    }

    public void UpdatePosition(Vector3 pos)
    {
        if (_brain != null)
        {
            _brain.moveToPos = true;
            _brain.posToMove = new Vector3(pos.x, _spawnedGuard.transform.position.y, pos.z);
            print("actualizo la pos");
        }
    }
}
