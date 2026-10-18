using System.Collections.Generic;
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

    private List<FishCard> _freeCards = new List<FishCard>();
    
    private List<FishCard> _busyCards = new List<FishCard>();
    
    private FishBucket _fishBucket;

    private bool _isOpened;


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

    public void RemoveFish()
    {
        
    }
}
