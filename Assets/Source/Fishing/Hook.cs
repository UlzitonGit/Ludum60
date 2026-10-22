using UnityEngine;
using Zenject;

public class Hook : MonoBehaviour
{
    [SerializeField] private Transform _hookSpp;
    
    [SerializeField] private Rigidbody _hookRb;
    
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _pickRadius;
    
    [SerializeField] private float _breakRadius;

    [SerializeField] private GameObject[] _lures;
    
    [SerializeField] private TutorialStage _stage;
    
    [SerializeField] private FishCard _fishCardPrefab;
    
    [SerializeField] private GameObject _pickingPart;
    
    private FishingRodController _fishingRodController;
    
    private TutorialController _tutorialController;
    
    private Fish _fish;
    
    private bool _isOnHook = false;

    private MainHeart _heart;
    
    private bool _isCasted;
    
    private bool _tutorial;
    
    private bool _tutorialHook;

    private bool _catchTutor;
    
    public bool IsOnHook => _isOnHook;

    [Inject]
    public void Construct(FishingRodController fishingRodController, MainHeart heart, TutorialController tutorialController)
    {
        _tutorialController = tutorialController;
        _heart = heart;
        _fishingRodController = fishingRodController;
    }

    public void HookInteract()
    {
        if (_isCasted && !_isOnHook && !_hookRb.isKinematic)
        {
           BackToSpp();
        }
        else if (_isCasted && _hookRb.isKinematic)
        {
            _hookRb.isKinematic = false;
            
            transform.parent = null;
        }
        else if (_isOnHook && _fish != null)
        {
            _hookRb.linearVelocity = new Vector3(0, 0, 0);
            _fish.SetIsStrike(true);
            _pickingPart.SetActive(false);
            
            if (!_tutorialHook && _tutorialController.GetStageIndex() == 4)
            {
                _stage.NextStage();
                _tutorialHook = true;
            }
        }
    }

    public void StartPicking()
    {
        _pickingPart.SetActive(true);
    }

    public void ApplyForwardImpulse(Transform target, float impulseForce)
    {
        if (target == null) return;
        
        
        _hookRb.AddForce(target.forward * impulseForce, ForceMode.Impulse);
    }

    public void MoveByTransform(Vector3 impulseForce)
    {
        transform.Translate(impulseForce * Time.deltaTime);
    }
    public void ApplyImpulseInRod(int forward, float impulseForce)
    {
        if(_fish == null) BackToSpp();
        print("strike");
        Vector3 posSpp = new Vector3(transform.position.x, 0, transform.position.z);
        
        Vector3 posRod = new Vector3(_hookSpp.position.x, 0, _hookSpp.position.z);
        
        Vector3 direction = (posRod - posSpp).normalized;
        
        if (forward == 1 && _fish != null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _pickRadius, _layerMask);
            
            _fishingRodController.PlayStrike();
            
            if (colliders.Length > 0)
            {
                BackToSpp(true);
            }

            _hookRb.AddForce(direction * forward * (impulseForce * _fish.GetForwardForce()) * _heart.GetRodPower(), ForceMode.Impulse);
        }
        if (forward == -1 && _fish != null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _breakRadius, _layerMask);
            
            _fishingRodController.PlayMiss();
            
            if (colliders.Length == 0)
            {
                BackToSpp();
            }
            _hookRb.AddForce(direction * forward * (impulseForce * _fish.GetBackwardForce()) / _heart.GetRodPower() , ForceMode.Impulse);
        }
        
        
    }
    
    

    private void BackToSpp(bool withFish = false)
    {
        if (!withFish && _fish != null)
        {
            _fish.BreakUp();
            _isOnHook = false;
            
            _fishingRodController.SetHasFish(false);
            
            _fish = null;
        }
        
        if (withFish)
        {
            if (!_catchTutor)
            {
                _stage.NextStage();
                _catchTutor = true;
            }
            _fishCardPrefab.gameObject.SetActive(true);
            _fishCardPrefab.ShowInfo(_fish.GetFishData().name, _fish.GetFishData().description,
                _fish.GetFishData().sprite, _fish.GetWeight(), 
                _fish.GetFishData().oxygenLevel, _fish.GetFishData().bloodLevel);
            _fish.Cached();
        }
        _fishingRodController.SetIsCasted(false);
        
        transform.position = _hookSpp.position;
        transform.parent = _hookSpp;
            
        _hookRb.isKinematic = true;
        _isCasted = false;
    }
    public void SetFish(Fish fish)
    {
        _fish = fish;
        
        _isOnHook = true;
        _fishingRodController.SetHasFish(_fish != null);
        if (fish == null)
        {
            _pickingPart.SetActive(false);
            _fishCardPrefab.gameObject.SetActive(false);
        }
        if (!_tutorial && _fish != null && _tutorialController.GetStageIndex() == 4)
        {
            _stage.NextStage();
            _tutorial = true;
        }
    }

    public Fish ThrowFish(Transform pos, bool rotate = false)
    {
        _fish.StoreFish(pos, rotate);
        _fish.SetIsInWater(true);
        _fishCardPrefab.gameObject.SetActive(false);
        
        Fish fish = _fish;
        _fishingRodController.SetHasFish(false);
        _fish = null;

        _isOnHook = false;
        
        return fish;
        
    }

    public void SetHook(int id)
    {
        foreach (var lure in _lures)
        {
            lure.SetActive(false);
        }
        _lures[id].SetActive(true);
    }

    public bool HasFish()
    {
        return _fish != null;
    }

    public Fish Getfish()
    {
        return _fish;
    }

    public void SetIsOnHook(bool isOnHook)
    {
        this._isOnHook = isOnHook;
    }

    public void SetIsCasted(bool isCasted)
    {
        this._isCasted = isCasted;
    }
}
