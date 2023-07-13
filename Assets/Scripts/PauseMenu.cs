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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


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
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Pause()
    {
        isGamePaused = true;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
