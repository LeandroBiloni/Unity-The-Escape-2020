using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActivateZones : MonoBehaviour
{
    public GameObject initialZone;
    public GameObject finalZone;
    public BoxCollider cantGoBack;
    public CheckpointManager checkpoint;
    public bool girlLoad;
    public bool boyLoad;
    public bool girlDelete;
    public bool boyDelete;
    public bool initialActive;
    public bool _changed;
    public GameObject checkpoinObject;

    private void Awake()
    {
        finalZone.SetActive(false);
        
    }
    // Start is called before the first frame update
    void Start()
    {

        checkpoinObject = GameObject.Find("CheckpointManager");
        checkpoint = checkpoinObject.GetComponent<CheckpointManager>();

        if (checkpoint.active1)
        {
            finalZone.SetActive(false);
            initialZone.SetActive(true);
        }

        if (checkpoint.active2)
        {
            initialZone.SetActive(false);
            finalZone.SetActive(true);
        }
    }

    private void Update()
    {
        if (_changed == false)
            ChangeActiveZone();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl"))
            girlLoad = true;

        if (other.CompareTag("Boy"))
            boyLoad = true;
    }

    public void ChangeActiveZone()
    {
        if (boyLoad || girlLoad)
        {
            finalZone.SetActive(true);
        }

        if (boyDelete && girlDelete)
        {
            initialZone.SetActive(false);
            cantGoBack.enabled = true;
            _changed = true;
        }
    }
}
