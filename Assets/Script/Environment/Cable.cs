using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{

	public bool activated;
	MeshRenderer _renderer;

	// Start is called before the first frame update
    void Start()
    {
		_renderer = GetComponent<MeshRenderer>();
		activated = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (activated)
		{
			ChangeColor();
		}
    }

	void ChangeColor()
	{
		_renderer.material.color = Color.green;
	}
}
