using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(AsyncLoadScene(sceneName));

    }

    private IEnumerator AsyncLoadScene(string sceneName)
    {

        SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSeconds(1);
    }
}
