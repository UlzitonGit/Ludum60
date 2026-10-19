using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [SerializeField] private Transform _scaleComponent;
    
    [SerializeField] private FishDataSO fishData;

    [SerializeField] private Vector3 fightDirection;
    [SerializeField] private GameObject _effects;
    
    private FishingLine _fishingLine;
    
    private float _weight;
    
    private Hook _hook;

    private Transform _targetPos;
    
    private bool _isOnHook;

    private bool _fightStarted;
    
    private bool _cached = false;
    
    private FishingMinigame _minigame;

    private bool _isStrike;
    private bool _isInWater;

    private int _wireReward;
    private int _nutsReward;
    private int _boltsReward;

    private float _backwardForce;
    private float _forwardForce;
    
    public void Initialize(Hook hook, FishingLine fishingLine, FishingMinigame minigame, bool isInWater = true)
    {
        _weight = Random.Range(fishData.minWeight, fishData.maxWeight);
        
        _scaleComponent.localScale = new Vector3(_weight,_weight, _weight);
        _fishingLine = fishingLine;
        _hook = hook;
        _minigame = minigame;
        
        _targetPos = _hook.transform;
        _isOnHook = true;
        
        _backwardForce = fishData._backwardForce * _weight;
        _forwardForce = fishData._forwardForce / (_weight / 2);

        if(fishData.bolts > 0) _boltsReward = fishData.bolts + (int)_weight;
        if(fishData.nuts > 0) _nutsReward = fishData.nuts + (int)_weight;
        if(fishData.wires > 0)_wireReward = fishData.wires + (int)_weight;
        
        _isInWater = isInWater;
        if(!isInWater) return;
        
        _hook.SetFish(this);
        _hook.SetIsOnHook(true);

        StartCoroutine(Picking());
    }

    private void Update()
    {
        if(!_isInWater) return;
        transform.position = _targetPos.transform.position;
        
        if(_isStrike && !_cached)
            _hook.MoveByTransform(fightDirection);
    }

    public void SetIsStrike(bool isStrike)
    {
        if(_isStrike) return;
        
        _isStrike = isStrike;
        
        StopAllCoroutines();
        
        _effects.SetActive(true);
        
        _minigame.gameObject.SetActive(true);
        
        if(!_fightStarted)
            StartCoroutine(ChangeDirection());
    }

    public void SetIsInWater(bool isInWater)
    {
        _isInWater = isInWater;
    }

    public void Cached()
    {
        _minigame.gameObject.SetActive(false);
        _cached = true;
        _effects.SetActive(false);
        StopAllCoroutines();
    }

    public void StoreFish(Transform pos, bool rotate = false)
    {
        _targetPos = pos;

        if (rotate == true)
        {
            transform.rotation = _targetPos.rotation;
        }
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
        return  _forwardForce;
    }

    public float GetBackwardForce()
    {
        return _backwardForce;
    }

    public FishDataSO GetFishData()
    {
        return fishData;
    }

    public int GetNuts()
    {
        return _nutsReward;
    }

    public int GetBolts()
    {
        return _boltsReward;
    }

    public int GetWires()
    {
        return _wireReward;
    }

    IEnumerator ChangeDirection()
    {
        _fightStarted = true;
        if (!_cached)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        
            fightDirection = new Vector3(fightDirection.x * -1, 0, fightDirection.z);
        
            StartCoroutine(ChangeDirection());
        }
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
