using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public Animator transition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }   else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Game Resumed");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Menu Loaded");
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(0));
    }

    public void QuitGame()
    {
        Debug.Log("Game Quitted");
        Application.Quit();
    }

    // this is supposed to be load level transition (fade)
    public IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(1f);

        //Load scene
        SceneManager.LoadScene(levelIndex);
    }
}
