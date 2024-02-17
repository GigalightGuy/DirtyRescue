using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject _pauseMenu;
    public static bool _isPaused;

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenu.SetActive(false);
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        _isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
