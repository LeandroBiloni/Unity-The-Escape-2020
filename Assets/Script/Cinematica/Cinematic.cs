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
	static bool alreadyPlayedFinal;

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
		if (!alreadyPlayedFinal)
		{
            cutSceneCam.gameObject.SetActive(true);
            thePlayer.gameObject.SetActive(false);
            StartCoroutine(FinishCut());
        }
    
    }

    IEnumerator FinishCut()
    {
        alreadyPlayedFinal = true;
        OnCinematicStart?.Invoke();
	    thePlayer.gameObject.SetActive(false);
	    yield return new WaitForSeconds(Wait);
        cutSceneCam.gameObject.SetActive(false);
        
        finalCam.gameObject.SetActive(true);
        
        var multipleCamera = finalCam.GetComponent<CameraMultipleTargets>();
        var girl = FindObjectOfType<Girl>().transform;
        var boy = FindObjectOfType<Boy>().transform;
        multipleCamera.AddTarget(girl);
        multipleCamera.AddTarget(boy);
        
        finalCam = Camera.main;
        OnCinematicEnd?.Invoke();
    }
}
