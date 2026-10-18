using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BucketUIController : MonoBehaviour
{

    [SerializeField] private GameObject _playerCam;
    [SerializeField] private GameObject _bucketCam;

    [SerializeField] private GameObject _canvas;
    
    [SerializeField] private FishCard _fishCard;
    
    [SerializeField] private Transform _fishCardTransform;

    [SerializeField] private Button _addFish;
    
    [SerializeField] private Button _removeFish;

    public List<FishCard> _freeCards = new List<FishCard>();
    
    public List<FishCard> _busyCards = new List<FishCard>();
    
    private FishBucket _fishBucket;

    private bool _isOpened;

    private int _returnId;

    [Inject]
    public void Construct(FishBucket fishBucket)
    {
        _fishBucket = fishBucket;

        for (int i = 0; i < 5; i++)
        {
            FishCard card = Instantiate(_fishCard.gameObject, _fishCardTransform).GetComponent<FishCard>();
            
            card.Init(i, this);
            
            _freeCards.Add(card);
            
        }
    }

    public void ShowBucket(bool hasFish = false)
    {
        if(_isOpened) return;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        _isOpened = true;
        _addFish.gameObject.SetActive(hasFish);
        _removeFish.gameObject.SetActive(false);
        
        _bucketCam.SetActive(true);
        _canvas.SetActive(true);
        
        _playerCam.SetActive(false);
    }

    public void HideBucket()
    {
        if(!_isOpened) return;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (var card in _busyCards)
        {
            card.UnSelect();
        }
        
        _isOpened = false;
        _bucketCam.SetActive(false);
        _canvas.SetActive(false);
        
        _playerCam.SetActive(true);
        
        _fishBucket.CloseStore();
    }


    public void AddFish()
    {
        Fish fish = _fishBucket.Store();
        
       if (fish == null) return;
       
       if (_freeCards.Count > 0)
       {
           FishCard card = _freeCards[0];
           
           card.ShowInfo(fish.GetFishData().name, fish.GetFishData().description, 
               fish.GetFishData().sprite, fish.GetWeight(),
               fish.GetFishData().oxygenLevel, fish.GetFishData().bloodLevel);
           
           _freeCards.RemoveAt(0);
           
           _busyCards.Add(card);
           
       }
    }

    public void FishCardPicked(int index)
    {
        HideAllSelect(index);
        
        int cardPos = 0;
        
        foreach (var card in _busyCards)
        {
            if (card.GetId() == index)
            {
                _removeFish.gameObject.SetActive(true);
                _addFish.gameObject.SetActive(false);
                _returnId = cardPos;
                break;
            }

            cardPos++;
        }
        
    }

    private void HideAllSelect(int i)
    {
        foreach (var card in _busyCards)
        {
            if(card.GetId() != i)
                card.UnSelect();
        }
    }

    public void RemoveFish()
    {
        if(_fishBucket.GetHook().HasFish()) return;
        
        FishCard card = _busyCards[_returnId];
        card.HideInfo();
        
        _busyCards.RemoveAt(_returnId);
        _freeCards.Add(card);
        
        _removeFish.gameObject.SetActive(false);
        _fishBucket.RemoveFish(_returnId);
        
    }
}
