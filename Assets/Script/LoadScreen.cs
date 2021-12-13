using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class LoadScreen : MonoBehaviour
{
    public Image loadBar;
    public string sceneToLoad;
    public Memory memory;
    public GameObject pressKey;
    public GameObject loading;
    public float progress;
    public AsyncOperation op;
    public float time;
    public bool called = false;
    // Start is called before the first frame update
    void Start()
    {
        memory = FindObjectOfType<Memory>();
        sceneToLoad = memory.activeLevel;
        pressKey.SetActive(false);
        //StartCoroutine(AsynchronousLoad(sceneToLoad));
        //op = SceneManager.LoadSceneAsync(sceneToLoad);
        //op.allowSceneActivation = false;
    }

     void Update()
    {
        time += Time.deltaTime;
        if (time >= 1 && called == false)
        {
            called = true;
            op = SceneManager.LoadSceneAsync(sceneToLoad);
            op.allowSceneActivation = false;
        }

        if (called)
        {
            if (!op.isDone)
            {
                // [0, 0.9] > [0, 1]
                progress = op.progress / 1;
                loadBar.fillAmount = progress;
            }

            // Loading completed
            if (op.progress == 0.9f)
            {
                loading.SetActive(false);
                pressKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                    op.allowSceneActivation = true;
            }
        }
    }
IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            progress = Mathf.Clamp01(ao.progress / 0.9f);
            loadBar.fillAmount = progress / 90;

            // Loading completed
            if (ao.progress == 0.9f)
            {
                loading.SetActive(false);
                pressKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                    ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
