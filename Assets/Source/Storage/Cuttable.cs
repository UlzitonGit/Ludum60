using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class Cuttable : MonoBehaviour
{
    [SerializeField] private GameObject _fishCanvas;
    
    [SerializeField] private Transform pos;

    [SerializeField] private Animator _hammerAnim;
    
    [SerializeField] private GameObject _playerCam;
    [SerializeField] private GameObject _bucketCam;
    
    [SerializeField] private ParticleSystem _fishParticles;
    
    [SerializeField] private TextMeshProUGUI _fishNuts;
    [SerializeField] private TextMeshProUGUI _fishWire;
    [SerializeField] private TextMeshProUGUI _fishBolts;
    
    [SerializeField] private Animator[] _bloodAnimator;
    
    private bool _isOpened;
    
    private Fish _fish;

    private int _fishMaxHp = 3;
    
    private int _fishHp;
    
    private Hook _hook;

    private bool _canHit;
    
    private InputHandler _inputHandler;
    
    private FishingRodController _fishingRodController;
    
    private ResourcesManager _resourcesManager;
    
    private bool inZone;

    private int effect;
    [Inject]
    public void Construct(InputHandler inputHandler, Hook hook, FishingRodController fishingRodController, ResourcesManager resourcesManager)
    {
        _resourcesManager = resourcesManager;
        
        _inputHandler = inputHandler;
        
        _fishingRodController = fishingRodController;

        _hook = hook;
        

        _inputHandler.OnInteract += OpenStore;

        _inputHandler.OnCast += Hit;
    }
    private void OpenStore(bool click)
    {
        if(!inZone || !_hook.HasFish()) return;

        effect = 0;
        _isOpened = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        _playerCam.SetActive(false);
        _bucketCam.SetActive(true);
        
        _fishingRodController.SetCanCast(false);
        
        _canHit = false;
        
        _fishHp = _fishMaxHp;
        
        _inputHandler.SetStopMovement(true);
        
        _fish = _hook.ThrowFish(pos, true);
        
        _fishCanvas.gameObject.SetActive(true);
        
        _hammerAnim.SetTrigger("Get");
        
        _hammerAnim.SetBool("Idle", true);

        _fishNuts.text = _fish.GetFishData().nuts.ToString();
        _fishWire.text = _fish.GetFishData().wires.ToString();
        _fishBolts.text = _fish.GetFishData().bolts.ToString();

        StartCoroutine(StartAnim());

    }

    public void Hit(bool click)
    {
        if (click && _canHit && _isOpened && _fish != null)
        {
            _hammerAnim.SetTrigger("Hit");
            _bloodAnimator[effect].SetTrigger("Blood");
            StartCoroutine(Delay());
            
            effect++;
        }
    }

    IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(2);
        _canHit = true;
    }

    IEnumerator Delay()
    {
        _canHit = false;
        
        yield return new WaitForSeconds(0.7f);

        _fishHp -= 1;
        
        _fishParticles.Play();
        
        if (_fishHp <= 0)
        {
            _resourcesManager.AddBolts(_fish.GetFishData().bolts);
            _resourcesManager.AddNuts(_fish.GetFishData().nuts);
            _resourcesManager.AddWires(_fish.GetFishData().wires);
            
            Destroy(_fish.gameObject);
            _fish = null;
            
        }
        
        yield return new WaitForSeconds(1f);
        
        _canHit = true;
        
    }

    public void CloseStore()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _isOpened = false;
        
        _fishCanvas.SetActive(false);
        
        _playerCam.SetActive(true);
        _bucketCam.SetActive(false);
        
        _fishingRodController.SetCanCast(true);
        
        _inputHandler.SetStopMovement(false);
        
        _hammerAnim.SetBool("Idle", false);
        
        
        if (_fish != null)
        {
            _fish.StoreFish(_hook.transform);
            _hook.SetFish(_fish);
            _fish = null;
        }
    }
    
    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract -= OpenStore;
            
            _inputHandler.OnCast -= Hit;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract += OpenStore;
            
            _inputHandler.OnCast += Hit;
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
}
