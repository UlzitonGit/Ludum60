using System;
using UnityEngine;
using Zenject;

public class FishingRodController : MonoBehaviour
{
    [SerializeField] private Animator _animatorRod;

    [SerializeField] private Transform _castSpp;

    [SerializeField] private Transform _camera;

    [SerializeField] private float _castSpeed;

    [SerializeField] private GameObject _lineSound;
    
    [SerializeField] private AudioPlayer _audioPlayer;
    
    [SerializeField] private AudioClip _strike;
    
    [SerializeField] private AudioClip _miss;

    [SerializeField] private AudioClip _cast;
    
    private Hook _hook;
    
    private InputHandler _inputHandler;
    
    private TutorialController _tutorialController;

    private bool _startCasting;

    private bool _isCasted;
    
    private float _castTimer;
    
    private float _maxCastTimer = 2;
    
    private bool _canCast = true;
    
    [Inject]
    public void Construct(InputHandler input, Hook hook, TutorialController tutorialController)
    {
        _tutorialController = tutorialController;
        
        _hook = hook;
        
        _inputHandler = input;

        _inputHandler.OnCast += Cast;
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnCast -= Cast;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnCast += Cast;
        }
    }

    private void FixedUpdate()
    {
        if (_startCasting && _castTimer < _maxCastTimer)
        {
            _castTimer += Time.fixedDeltaTime;
        }
    }

    public void PlayStrike()
    {
        _audioPlayer.Play(_strike);
    }

    public void PlayMiss()
    {
        _audioPlayer.Play(_miss);
    }

    private void Cast(bool cast)
    {
        if(!_canCast || _tutorialController.GetStageIndex() < 4) return;
        if (_hook.IsOnHook && cast)
        {
            _hook.HookInteract();
            
            _animatorRod.SetTrigger("Strike");
        }
        
        else if (_isCasted && cast && !_hook.IsOnHook)
        {
            _isCasted = false;
            
            _hook.SetIsCasted(false);
            
            _hook.HookInteract();
            
            _castTimer = 0;
            
        }
        else if (cast && !_startCasting && !_isCasted)
        {
            _startCasting = true;

            _isCasted = false;
            
            _hook.SetIsCasted(false);
            
            _animatorRod.SetBool("Casting", true);

            _castTimer = 0;
        }
        else if (!cast && _startCasting  && !_isCasted)
        {
            _startCasting = false;
            
            _hook.SetIsCasted(true);
            
            _hook.SetIsOnHook(false);
            
            _isCasted = true;

            _hook.HookInteract();
            
            _hook.ApplyForwardImpulse(_camera, _castTimer * _castSpeed);

            _audioPlayer.Play(_cast);
            
            _animatorRod.SetBool("Casting", false);
        }
        
        _lineSound.SetActive(_hook.IsOnHook);
    }

    public void SetHasFish(bool hasFish)
    {
        _animatorRod.SetBool("HasFish", hasFish);
        
    }

    public void SetCanCast(bool canCast)
    {
        _canCast = canCast;
    }

    public void SetIsCasted(bool isCasted)
    {
        _isCasted = isCasted;
        
        _startCasting = false;
    }
}
