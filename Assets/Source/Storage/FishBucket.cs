using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FishBucket : MonoBehaviour, IStorage
{
    [SerializeField] private Hook _hook;
    
    private List<Fish> _fishes;
    
    private InputHandler _inputHandler;

    private bool inZone;

    [Inject]
    public void Construct(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;

        _inputHandler.OnInteract += Store;
    }


    List<Fish> IStorage.fishes
    {
        get => _fishes;
        set => _fishes = value;
    }

    private void Store(bool clicked)
    {
        if (inZone && _hook.HasFish())
        {
            _hook.ThrowFish(transform);
        }
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
            _inputHandler.OnInteract -= Store;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract += Store;
        }
    }
}
