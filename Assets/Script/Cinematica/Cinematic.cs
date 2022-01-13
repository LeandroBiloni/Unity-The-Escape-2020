using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public Camera thePlayer;
    public Camera firstCinematic;
    public Camera cutSceneCam;
    public Camera finalCam;
    public float Wait;
    public float timerFirstCinematic;

	static bool alreadyPlayed;

	public delegate void MyCinematic();

	public event MyCinematic OnCinematicEnd;
	public event MyCinematic OnCinematicStart;

    // Start is called before the first frame update
    void Start()
    {
		if (!alreadyPlayed)
		{
			firstCinematic.gameObject.SetActive(true);
			thePlayer.gameObject.SetActive(false);
			StartCoroutine(FirstCut());
			
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FirstCut()
    {
	    yield return new WaitForEndOfFrame();
	    OnCinematicStart?.Invoke();
		alreadyPlayed = true;
        yield return new WaitForSeconds(timerFirstCinematic);
        firstCinematic.gameObject.SetActive(false);
        thePlayer.gameObject.SetActive(true);
		thePlayer = Camera.main;
		OnCinematicEnd?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        cutSceneCam.gameObject.SetActive(true);
        thePlayer.gameObject.SetActive(false);
        StartCoroutine(FinishCut());
    }

    IEnumerator FinishCut()
    {
	    OnCinematicStart?.Invoke();
        yield return new WaitForSeconds(Wait);
		thePlayer.gameObject.SetActive(false);
        cutSceneCam.gameObject.SetActive(true);
        finalCam.gameObject.SetActive(true);
        finalCam = Camera.main;
        OnCinematicEnd?.Invoke();
    }
}
