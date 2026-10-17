using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [SerializeField] private Transform _scaleComponent;

    [SerializeField] private float _forwardForce;
    
    [SerializeField] private float _backwardForce;
    
    private FishingLine _fishingLine;
    
    private float _weight;
    
    private Hook _hook;
    
    private bool _isOnHook;
    
    private FishingMinigame _minigame;

    private bool _isStrike;
    
    public void Initialize(Hook hook, FishingLine fishingLine, FishingMinigame minigame)
    {
        _weight = Random.Range(1, 3);
        
        _scaleComponent.localScale = new Vector3(0.5f * _weight, 0.5f * _weight,0.5f * _weight);
        _fishingLine = fishingLine;
        _hook = hook;
        _minigame = minigame;
        
        _hook.SetFish(this);
        _hook.SetIsOnHook(true);
        _isOnHook = true;
        
        _backwardForce *= _weight;
        _forwardForce /= _weight;

        StartCoroutine(Picking());
    }

    private void Update()
    {
        transform.position = _hook.transform.position;
    }

    public void SetIsStrike(bool isStrike)
    {
        if(_isStrike) return;
        
        _isStrike = isStrike;
        
        StopAllCoroutines();
        
        _minigame.gameObject.SetActive(true);
    }

    public void Cached()
    {
        _minigame.gameObject.SetActive(false);
        
        StopAllCoroutines();
    }

    public void StoreFish(Transform pos)
    {
        _hook.transform.position = pos.position;
        
        gameObject.SetActive(false);
    }
    public void BreakUp()
    {
        _minigame.gameObject.SetActive(false);
        
        StopAllCoroutines();
        
        Destroy(gameObject);
    }

    public float GetWeight()
    {
        return _weight;
    }

    public float GetForwardForce()
    {
        return _forwardForce;
    }

    public float GetBackwardForce()
    {
        return _backwardForce;
    }

    IEnumerator Picking()
    {
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.5f);
            _fishingLine.Yank(_weight * 2);
        }
        _hook.SetFish(null);
        _hook.SetIsOnHook(false);
        
        Destroy(gameObject);
    }
}
