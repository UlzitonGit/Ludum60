using UnityEngine;
using Zenject;

public abstract class InteractiveTable : MonoBehaviour
{
    [SerializeField] protected GameObject _canvas;
     
    
    [SerializeField] protected GameObject _playerCam;
    [SerializeField] protected GameObject _interactiveCam;
    
    protected bool _isOpened;

    [SerializeField] protected GameObject _hint;
    
    protected InputHandler _inputHandler;
    
    protected bool inZone;
    
    [Inject]
    public virtual void Construct(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;

        _inputHandler.OnInteract += OpenStore;
    }
    protected virtual void OpenStore(bool click)
    {
        if(!inZone) return;
        
        _isOpened = true;
        
        _canvas.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _playerCam.SetActive(false);
        
        _interactiveCam.SetActive(true);
        
        _inputHandler.SetStopMovement(true);

    }
    

    public virtual void CloseStore()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _hint.SetActive(false);
        _isOpened = false;
        
        _canvas.SetActive(false);
        
        _playerCam.SetActive(true);
        _interactiveCam.SetActive(false);
        
        _inputHandler.SetStopMovement(false);
    }
    
    protected virtual void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract -= OpenStore;
        }
    }

    protected virtual void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract += OpenStore;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            _hint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            _hint.SetActive(false);
        }
    }
}
