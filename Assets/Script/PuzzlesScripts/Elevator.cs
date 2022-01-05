using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float _speed;
    private HashSet<ElevatorButton> _buttons = new HashSet<ElevatorButton>();
    

    IEnumerator Move()
    {
        while (true)
        {
            transform.position += transform.up * (_speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void AddButton(ElevatorButton button)
    {
        _buttons.Add(button);
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
