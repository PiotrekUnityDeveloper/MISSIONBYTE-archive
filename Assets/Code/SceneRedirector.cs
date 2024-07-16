using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRedirector : MonoBehaviour
{
    public string sceneName;
    public float delay;
    public bool skipOnEnter;
    public bool skipOnSpace;

    void Start()
    {
        StartCoroutine(RedirectToScene());
    }

    private void Update()
    {
        if(skipOnEnter == true && Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine(RedirectToScene());
            SceneManager.LoadSceneAsync(sceneName);
        }else if (skipOnSpace == true && Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(RedirectToScene());
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

    private IEnumerator RedirectToScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
    }

}
