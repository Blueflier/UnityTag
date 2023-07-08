using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject PauseMenuUI;
    public string MainMenu;

    void Start()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (isGamePaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
            
    }

    public void Resume()
    {
        isGamePaused = false;
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;

    }
    void Pause()
    {
        isGamePaused = true;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;

    }

    public void LoadMenu()
    {
        PauseMenuUI.SetActive(false);
        SceneManager.LoadScene(MainMenu);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }

}
