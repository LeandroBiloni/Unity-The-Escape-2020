using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
	[SerializeField] private Color _activeColor;
	private MeshRenderer _renderer;
	private Color _defaultColor;

	// Start is called before the first frame update
    void Start()
    {
		_renderer = GetComponent<MeshRenderer>();
		_defaultColor = _renderer.material.color;
    }

    public void Activate()
	{
		_renderer.material.color = _activeColor;
	}

    public void Deactivate()
    {
	    _renderer.material.color = _defaultColor;
    }
}
