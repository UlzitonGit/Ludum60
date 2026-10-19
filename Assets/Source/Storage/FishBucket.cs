using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FishBucket : MonoBehaviour, IStorage
{
    [SerializeField] private BucketUIController _bucketUIController;
    
    [SerializeField] private List<Transform> _fishSpp = new List<Transform>();
    
    [SerializeField] private TutorialStage _tutorialStage;

    [SerializeField] private GameObject _hint;
    
    
    private Hook _hook;
    
    private List<Fish> _fishes = new List<Fish>(5);
    
    private InputHandler _inputHandler;

    private bool inZone;
    
    private TutorialController _tutorialController;
    
    public Action<bool> OnInteract { get; set; }

    [Inject]
    public void Construct(InputHandler inputHandler, Hook hook, TutorialController tutorialController)
    {
        _inputHandler = inputHandler;

        _tutorialController = tutorialController;
        
        _hook = hook;

        _inputHandler.OnInteract += OpenStore;
    }


    List<Fish> IStorage.fishes
    {
        get => _fishes;
        set => _fishes = value;
    }

    private void OpenStore(bool click)
    {
        if(!inZone && !click) return;
        
        _inputHandler.SetIsStoped(true);
        
        _bucketUIController.ShowBucket(_hook.HasFish());
        OnInteract?.Invoke(true);
    }

    public Fish Store(bool isTutor = false)
    {
   
        if ((inZone || isTutor) && _fishes.Count < 5)
        {
            _fishes.Add(_hook.Getfish());
            return _hook.ThrowFish(_fishSpp[_fishes.Count - 1]);
        }

        return null;
    }

    public void RemoveFish(int fishId)
    {
        if(_hook.HasFish()) return;
        
        _fishes[fishId].StoreFish(_hook.transform);
        
        _hook.SetFish( _fishes[fishId]);
        _fishes.RemoveAt(fishId);

        if (_tutorialController.IsTutorial())
        {
            _tutorialStage.NextStage();
        }
    }

    public void CloseStore()
    {
        _inputHandler.SetIsStoped(false);
        
        _hint.SetActive(false);
        
        OnInteract?.Invoke(false);
    }

    public Hook GetHook()
    {
        return _hook;
    }

    public BucketUIController GetBucketUIController()
    {
        return _bucketUIController;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _hint.SetActive(true);
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _hint.SetActive(false);
            inZone = false;
        }
    }
    
    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract -= OpenStore;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract += OpenStore;
        }
    }
}
