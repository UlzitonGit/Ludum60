using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FishBucket : MonoBehaviour, IStorage
{
    [SerializeField] private BucketUIController _bucketUIController;
    
    [SerializeField] private List<Transform> _fishSpp = new List<Transform>();
    
    private Hook _hook;
    
    private List<Fish> _fishes = new List<Fish>();
    
    private InputHandler _inputHandler;

    private bool inZone;
    
    public Action<bool> OnInteract { get; set; }

    [Inject]
    public void Construct(InputHandler inputHandler, Hook hook)
    {
        _inputHandler = inputHandler;

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

    public Fish Store()
    {
   
        if (inZone && _fishes.Count < 5)
        {
            _fishes.Add(_hook.Getfish());
            return _hook.ThrowFish(_fishSpp[_fishes.Count - 1]);
        }

        return null;
    }

    public void CloseStore()
    {
        _inputHandler.SetIsStoped(false);
        
        OnInteract?.Invoke(false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
