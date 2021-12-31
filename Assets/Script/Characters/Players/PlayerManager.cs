using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public BoyBrain boy;
    public GirlBrain girl;
    public bool isChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        boy.selected = true;
	}

    // Update is called once per frame
    void Update()
    {
        CheckKeys();
	}

    //HACE EL CAMBIO ENTRE LOS PERSONAJES
    private void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isChanged)
            {
                boy.selected = true;
                girl.selected = false;
                isChanged = false;
            }   
            else
            {
                girl.selected = true;
                boy.selected = false;
                isChanged = true;
            }
			
		}
    }
}
