using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class LuresCraft : InteractiveTable
{
    [SerializeField] private Lure[] _lures;


    [SerializeField] private GameObject _craftInfoPanel;
    [SerializeField] private TextMeshProUGUI _wireText;
    [SerializeField] private TextMeshProUGUI _boltText;
    [SerializeField] private TextMeshProUGUI _hutsText;
    
    [SerializeField] private Animator _animator;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private GameObject _chooseButton;
    [SerializeField] private GameObject _craftButton;
    
    [SerializeField] private FishArea  _fishArea;
    
    private ResourcesManager _resourcesManager;
    
    private Lure _equipedLure;

    private Hook _hook;

    private int _observingLure;
    
    private Lure _activeLure;
    
    private bool _isCrafting;
    
    [Inject]
    public void Construct(InputHandler inputHandler, ResourcesManager resourcesManager, Hook hook)
    {
        _resourcesManager = resourcesManager;
        
        _inputHandler = inputHandler;

        _inputHandler.OnInteract += OpenStore;
        
        _activeLure = _lures[0];
        
        _equipedLure = _activeLure;
        
        _hook = hook;
        
        _fishArea.SetActiveLure(_equipedLure.LuresSO());
        
        ChooseLure();
    }

    protected override void OpenStore(bool click)
    {
        base.OpenStore(click);
        
        _hint.SetActive(false);
        
        SetLureInfo();
    }

    public void CheckNextLure(int next)
    {
        if(_isCrafting) return;
        
        _observingLure += next;
        if(_observingLure < 0) _observingLure = _lures.Length - 1;
        if(_observingLure >= _lures.Length) _observingLure = 0;
        
        SetLureInfo();
    }

    public override void CloseStore()
    {
        if(_isCrafting) return;
        base.CloseStore();
        _canvas.SetActive(false);
        
        print("close store");
    }

    public void ChooseLure()
    {
        _equipedLure = _activeLure;

        int i = 0;

        foreach (Lure lure in _lures)
        {
            if (lure.LuresSO() == _equipedLure.LuresSO())
            {
                _hook.SetHook(i);
            }

            i++;
        }
        
        SetLureInfo();
    }

    public void TryCraft()
    {
        if(_isCrafting) return;

        if (_activeLure.LuresSO().Bolts <= _resourcesManager.GetBolts() &&
            _activeLure.LuresSO().Huts <= _resourcesManager.GetNuts() &&
            _activeLure.LuresSO().Wires <= _resourcesManager.GetWires())
        {
            _resourcesManager.AddBolts(_activeLure.LuresSO().Bolts * -1);
            _resourcesManager.AddNuts(_activeLure.LuresSO().Huts * -1);
            _resourcesManager.AddWires(_activeLure.LuresSO().Wires * -1);
            
            StartCoroutine(Crafting());
        }
    }

    IEnumerator Crafting()
    {
        _animator.SetTrigger("crafting");
        _isCrafting = true;
        _activeLure.gameObject.SetActive(false);
        _craftInfoPanel.SetActive(false);
        
        yield return new WaitForSeconds(2.6f);
        
        _craftInfoPanel.SetActive(false);
        _activeLure.SetCreated(true);
        _activeLure.gameObject.SetActive(true);
        _isCrafting = false;
        
        SetLureInfo();
    }

    private void SetLureInfo()
    {
        if (_activeLure != null)
        {
            _activeLure.gameObject.SetActive(false);
        }
        
        _activeLure = _lures[_observingLure];
        _lures[_observingLure].gameObject.SetActive(true);
        
        _name.text = _activeLure.LuresSO().name;

        if (_activeLure.LuresSO().Wires == 0 && _activeLure.LuresSO().Bolts == 0 && _activeLure.LuresSO().Huts == 0)
        {
            _activeLure.SetCreated(true);
        }
        
        _fishArea.SetActiveLure(_equipedLure.LuresSO());
        
        if (_activeLure.IsCreated())
        {
            _craftButton.SetActive(false);
            _craftInfoPanel.SetActive(false);
            
            if (_activeLure.LuresSO().name != _equipedLure.LuresSO().name)
            {
                _chooseButton.SetActive(true);
            }
            else
            {
                _chooseButton.SetActive(false);
            }
            
            return;
        }
        
        
        _chooseButton.SetActive(false);
        _craftButton.SetActive(true);
        _craftInfoPanel.SetActive(true);
        
        _wireText.text = _activeLure.LuresSO().Wires.ToString();
        _hutsText.text = _activeLure.LuresSO().Huts.ToString();
        _boltText.text = _activeLure.LuresSO().Bolts.ToString();
        
    }
}
