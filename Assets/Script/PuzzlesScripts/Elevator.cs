using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private List<Button> _buttons = new List<Button>();

    // Update is called once per frame
    void Update()
    {
        // if (buttonA._active == true)
        //     aStepped = true;
        // else aStepped = false;
        //
        // if (buttonB._active == true)
        //     bStepped = true;
        // else bStepped = false;
        //
        // if (aStepped && bStepped)
        //     Move();

    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.position += transform.up * (_speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    
    public void CheckButtons()
    {
        var count = 0;
        foreach (var button in _buttons)
        {
            if (!button.IsActive()) return;

            count++;
            
            if (count == _buttons.Count)
                StartCoroutine(Move());
        }
    }
}
