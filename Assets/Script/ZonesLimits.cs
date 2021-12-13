using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonesLimits : MonoBehaviour
{
    public ActivateZones activator;
    public bool loadBoy;
    public bool deleteBoy;
    public bool loadGirl;
    public bool deleteGirl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl"))
        {
            if (loadGirl)
            {
                activator.girlLoad = true;
            }

            if (deleteGirl)
            {
                activator.girlDelete = true;
            }
        }

        if (other.CompareTag("Boy"))
        {
            if (loadBoy)
            {
                activator.boyLoad = true;
            }

            if (deleteBoy)
            {
                activator.boyDelete = true;
            }
        }
    }
}
