using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    public void LoadSceneTransition(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncTransition(sceneName));

    }

    private IEnumerator LoadSceneAsyncTransition(string sceneName)
    {
        transitionAnim.gameObject.SetActive(true);
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(sceneName);
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        transitionAnim.gameObject.SetActive(false);
    }
}
