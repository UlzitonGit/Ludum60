using System;
using UnityEngine;
using Zenject;

public class Dialog : MonoBehaviour, IStage
{
    [SerializeField] private GameObject dialogs;
    
    [SerializeField] private GameObject _playerCam;
    [SerializeField] private GameObject _dialogCam;
    private InputHandler _inputHandler;

   
    public void SetUP(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        gameObject.SetActive(false);
    }
 

    public void StartStage()
    {
        _playerCam.SetActive(false);
        _dialogCam.SetActive(true);
        _inputHandler.SetStopMovement(true);
        dialogs.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndStage()
    {
        _playerCam.SetActive(true);
        _dialogCam.SetActive(false);
        dialogs.SetActive(false);
        _inputHandler.SetStopMovement(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
