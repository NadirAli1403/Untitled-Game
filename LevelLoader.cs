using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Update is called once per frame

    public Animator transition;

    public float transitionTime = 1f;

    [SerializeField] public Player player;

    private bool isDoorOverlap;

    public bool nextLevel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && isDoorOverlap)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isDoorOverlap = true;
        }
    }
}
