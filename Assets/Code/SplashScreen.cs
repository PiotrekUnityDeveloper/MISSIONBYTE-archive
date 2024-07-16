using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public GameObject circle;

    public bool playing = false;

    public GameObject mainLabel;
    public GameObject playLabel;

    public GameObject fadeOut;

    public string targetSceneName;

    public bool theresnocomingback = false;

    public Slider codeTypeSlider;

    public Slider transitionSpeed;

    public void ChangeTransitionSpeed()
    {
        SettingsManager1.matrixFlashTime = transitionSpeed.value;
        SettingsManager1.matrixMinDelay = transitionSpeed.value;
        SettingsManager1.matrixMaxDelay = transitionSpeed.value + 1;
    }

    public void CodeTypeChanged()
    {
        if(codeTypeSlider.value == 0)
        {
            //flicker
            circle.tag = "FadeFlash";
        }
        else
        {
            //dissolve
            circle.tag = "Flash";
        }
    }

    public GameObject loading;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("startlevel", 0);
        StartCoroutine(RedirectToMenu());
    }

    private IEnumerator RedirectToMenu()
    {
        yield return new WaitForSeconds(5f);

        if (playing == false)
        {
            theresnocomingback = true;
            fadeOut.SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            loading.SetActive(true);
            yield return new WaitForSeconds(Random.Range(4f, 5f));
            
            SceneManager.LoadSceneAsync(targetSceneName);
        }

    }

    private IEnumerator ForceRedirectToMenu()
    {
        yield return new WaitForSeconds(Random.Range(5f, 6.5f));

        SceneManager.LoadScene(targetSceneName);

    }

    // Update is called once per frame
    void Update()
    {
        if (playing && Input.GetKeyDown(KeyCode.Return) && theresnocomingback == false)
        {
            //exit
            fadeOut.SetActive(true);
            loading.SetActive(true);
            playing = false;
            StartCoroutine(ForceRedirectToMenu());
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            playing = true;
            mainLabel.gameObject.SetActive(false);
            playLabel.gameObject.SetActive(true);
            codeTypeSlider.gameObject.SetActive(true);
        }

        

        if(playing == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            circle.GetComponent<Animator>().SetTrigger("boom");
        }

        //follow the mouse

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z += Camera.main.nearClipPlane;
        circle.transform.position = mousePosition;
    }
}
