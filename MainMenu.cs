using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static System.TimeZoneInfo;
using System;

public class MainMenu : MonoBehaviour

{
    public Animator transition;

    public float transitionTime = 1f;

    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadLevel(1));
        Debug.Log("Went to" + sceneName);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(levelIndex);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application has quit");
    }
}
