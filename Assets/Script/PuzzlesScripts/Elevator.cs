using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed;
    public Button buttonA;
    public bool aStepped;
    public bool bStepped;
    public Button buttonB;
    private bool _canMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonA.stepped == true)
            aStepped = true;
        else aStepped = false;

        if (buttonB.stepped == true)
            bStepped = true;
        else bStepped = false;

        if (aStepped && bStepped)
            Move();

    }

    private void Move()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
