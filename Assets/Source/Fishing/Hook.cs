using UnityEngine;
using Zenject;

public class Hook : MonoBehaviour
{
    [SerializeField] private Transform _hookSpp;
    
    [SerializeField] private Rigidbody _hookRb;
    
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _pickRadius;
    
    [SerializeField] private float _breakRadius;
    
    private FishingRodController _fishingRodController;
    
    private Fish _fish;
    
    private bool _isOnHook = false;
    
    private bool _isCasted;
    
    public bool IsOnHook => _isOnHook;

    [Inject]
    public void Construct(FishingRodController fishingRodController)
    {
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
        }
    }

    public void ApplyForwardImpulse(Transform target, float impulseForce)
    {
        if (target == null) return;
        
        _hookRb.AddForce(target.forward * impulseForce, ForceMode.Impulse);
    }
    
    public void ApplyImpulseInRod(int forward, float impulseForce)
    {
        
        print("strike");
        _hookRb.linearVelocity = new Vector3(0, 0, 0);
        Vector3 posSpp = new Vector3(transform.position.x, 0, transform.position.z);
        
        Vector3 posRod = new Vector3(_hookSpp.position.x, 0, _hookSpp.position.z);
        
        Vector3 direction = (posRod - posSpp).normalized;
        
        if (forward == 1)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _pickRadius, _layerMask);

            if (colliders.Length > 0)
            {
                BackToSpp(true);
            }

            _hookRb.AddForce(direction * forward * (impulseForce * _fish.GetForwardForce()), ForceMode.Impulse);
        }
        if (forward == -1)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _breakRadius, _layerMask);

            if (colliders.Length == 0)
            {
                BackToSpp();
            }
            _hookRb.AddForce(direction * forward * (impulseForce * _fish.GetBackwardForce()), ForceMode.Impulse);
        }
        
        
    }
    
    

    private void BackToSpp(bool withFish = false)
    {
        if (!withFish && _fish != null)
        {
            _fish.BreakUp();
            _isOnHook = false;
            
            _fish = null;
        }
        
        if (withFish)
        {
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
    }

    public Fish ThrowFish(Transform pos, bool rotate = false)
    {
        _fish.StoreFish(pos, rotate);
        
        Fish fish = _fish;
        
        _fish = null;

        _isOnHook = false;
        
        return fish;
        
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
