using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDoor : MonoBehaviour
{
    public Door door;
    public Platform platform;
    public GameObject command;
	public Transform cableContainer;
	public Transform[] myCables;
    public List<Cable> cables = new List<Cable>();
    public Color cablesDefaultColor;

    private void Start()
	{
		myCables = new Transform[cableContainer.childCount];
		for (int i = 0; i < myCables.Length; i++)
		{
			myCables[i] = cableContainer.GetChild(i);
			var myCable = myCables[i].gameObject.GetComponent<Cable>();
			cables.Add(myCable);
		}
	}
	/*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Scientific" && door != null)
		{
            command.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
            door.openThroughComputer = true;
			door.audioManager.PlaySFX(door.slideDoor, 1.7f);
            }
		}

        if (collision.gameObject.tag == "Scientific" && platform != null)
            platform.move = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Scientific" && door != null)
        {
            command.SetActive(false);
        }
    }*/
	private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Scientific") && door != null)
        {
            ScientificBrain sci = other.GetComponent<ScientificBrain>();
            if (sci.controlled)
            {
                command.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
					
					door.openThroughComputer = true;
					door.audioManager.PlaySFX(door.slideDoor, 1.7f);                  
                    for (int i = 0; i < cables.Count; i++)
					{
						cables[i].activated = true;
					}
                }
            }
        }

        if (other.gameObject.CompareTag("Scientific") && platform != null)
        {
            ScientificBrain sci = other.GetComponent<ScientificBrain>();
            if (sci.controlled)
            {
                command.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {

                    platform.move = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Scientific"))
        {
            command.SetActive(false);
        }
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
    