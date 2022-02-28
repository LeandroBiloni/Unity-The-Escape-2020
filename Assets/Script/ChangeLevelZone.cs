using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeLevelZone : MonoBehaviour
{
    private ScenesManager _scenesManager;
    public string nextLevel;
    public float loadDelay;

    [SerializeField] private List<EndGuard> _guards;
    private void Start()
    {
        _scenesManager = FindObjectOfType<ScenesManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(LoadNextLevelDelay());
            foreach (var g in _guards)
            {
                g.Activate();
                var boy = FindObjectOfType<Boy>();
                boy.StopMovement();
                boy.CancelSelection();
                boy.FieldOfViewOff();
                var girl = FindObjectOfType<Girl>();
                girl.StopMovement();
                girl.CancelSelection();
                girl.FieldOfViewOff();
            }
        }
    }

    IEnumerator LoadNextLevelDelay()
    {
        yield return new WaitForSeconds(loadDelay);
        
        Memory.Instance.OnSceneChange(nextLevel);
        _scenesManager.LoadNextLevel();
    }
}
