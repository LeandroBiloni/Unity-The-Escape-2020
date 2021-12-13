using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    static Memory memory;

    public string activeLevel;

    private void Awake()
    {
        if (memory == null)
        {
            memory = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (memory != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
