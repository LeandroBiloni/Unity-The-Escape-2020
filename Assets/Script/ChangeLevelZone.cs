using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelZone : MonoBehaviour
{
    private ScenesManager _scenesManager;
    public string nextLevel;
    public float loadDelay;
    private void Start()
    {
        _scenesManager = FindObjectOfType<ScenesManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(LoadNextLevelDelay());
        }
    }

    IEnumerator LoadNextLevelDelay()
    {
        yield return new WaitForSeconds(loadDelay);
        
        Memory.Instance.OnSceneChange(nextLevel);
        _scenesManager.LoadNextLevel();
    }
}
