using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnSceneHandler : MonoBehaviour
{
    public Scene cs, ns;

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ChangeScene()
    {
        cs = SceneManager.GetActiveScene();
        if(cs.name == "SampleScene")
        {
            SceneManager.LoadScene("SampleScene2");
            ns = SceneManager.GetSceneByName("SampleScene2");
            StartCoroutine(SetActive(ns));
        }
        else if(cs.name == "SampleScene2")
        {
            SceneManager.LoadScene("SampleScene");
            ns = SceneManager.GetSceneByName("SampleScene");
            StartCoroutine(SetActive(ns));
        }
    }

    public IEnumerator SetActive(Scene s)
    {
        int i = 0;
        while (i == 0)
        {
            i++;
            yield return null;
        }
        SceneManager.SetActiveScene(s);
        yield break;
    }
}
