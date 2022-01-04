using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool _move;
    [SerializeField] private float _moveTime;
    public float speed;
    [SerializeField] private GameObject limits;
    private bool _interactable;

    public void StartMovement()
    {
        if (!_interactable) return;

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        var time = 0f;

        while (time <= _moveTime)
        {
            transform.position += transform.up * (speed * Time.deltaTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        limits.SetActive(false);
    }

    public bool IsInteractable()
    {
        return _interactable;
    }
}
