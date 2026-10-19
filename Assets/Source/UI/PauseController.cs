using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private InputHandler _inputHandler;

    private bool _started;
    
    [Inject]
    public void Construct(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;

        _inputHandler.OnPause += ShowPauseMenu;
    }

  

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnPause -= ShowPauseMenu;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnPause += ShowPauseMenu;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void ShowPauseMenu(bool click)
    {
        _started = !_started;
        _pauseMenu.SetActive(true);
        print(_started);
       
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        
    }
}
