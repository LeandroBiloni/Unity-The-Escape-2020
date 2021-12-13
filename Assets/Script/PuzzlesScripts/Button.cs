using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Button : MonoBehaviour
{
    public bool stepped = false;
	public Transform cableContainer;
	//public Transform[] myCables;
	public List<Cable> cables = new List<Cable>();
    public GameObject commandIcon;
    public BoyBrain boy;
    public float distanceFromPlayer;
    public bool playerCanUse;
    public Color cablesDefaultColor;

    private void Start()
    {
        boy = FindObjectOfType<BoyBrain>();
		//myCables = new Transform[cableContainer.childCount];
		//for (int i = 0; i < myCables.Length; i++)
		//{
		//	myCables[i] = cableContainer.GetChild(i);
		//	var myCable = myCables[i].gameObject.GetComponent<Cable>();
		//	cables.Add(myCable);
		//}
	}

    private void Update()
    {
        if (commandIcon)
        {
            if (boy.holding)
            {
                if (Vector3.Distance(boy.transform.position, transform.position) <= distanceFromPlayer)
                {
                    commandIcon.SetActive(true);
                }
            }
            else commandIcon.SetActive(false);
        }

		if (stepped)
		{
			for (int i = 0; i < cables.Count; i++)
			{
				cables[i].activated = true;
			}
		}
		else
		{
			for (int i = 0; i < cables.Count; i++)
			{
				cables[i].activated = false;
			}
		}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects") || (other.gameObject.layer == LayerMask.NameToLayer("Player") && playerCanUse))
        {
            stepped = true;            
            if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
            {
                other.gameObject.transform.position = transform.position;
                other.attachedRigidbody.isKinematic = true;
            }
            CablesOn();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovableObjects") || (other.gameObject.layer == LayerMask.NameToLayer("Player") && playerCanUse))
        {
            stepped = false;      
            if(other.gameObject.layer == LayerMask.NameToLayer("MovableObjects"))
            {
                other.attachedRigidbody.isKinematic = false;
            }
        }
        CablesOff();
    }

    private void CablesOn()
    {
        foreach (var cable in cables)
        {
            var render = cable.gameObject.GetComponent<Renderer>();
            Material[] materials = render.materials;
            materials[0].color = Color.green;
        }
    }

    private void CablesOff()
    {
        foreach (var cable in cables)
        {
            var render = cable.gameObject.GetComponent<Renderer>();
            Material[] materials = render.materials;
            materials[0].color = cablesDefaultColor;
        }
    }
}
