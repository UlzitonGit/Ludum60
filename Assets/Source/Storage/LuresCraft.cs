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

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private GameObject _chooseButton;
    [SerializeField] private GameObject _craftButton;
    
    private ResourcesManager _resourcesManager;

    private int _observingLure;
    
    private Lure _activeLure;
    
    [Inject]
    public void Construct(InputHandler inputHandler, ResourcesManager resourcesManager)
    {
        _resourcesManager = resourcesManager;
        
        _inputHandler = inputHandler;

        _inputHandler.OnInteract += OpenStore;
    }

    protected override void OpenStore(bool click)
    {
        base.OpenStore(click);
        
        
        
        SetLureInfo();
    }

    public void CheckNextLure(int next)
    {
        _observingLure += next;
        if(_observingLure < 0) _observingLure = _lures.Length - 1;
        if(_observingLure >= _lures.Length) _observingLure = 0;
        
        SetLureInfo();
    }

    public override void CloseStore()
    {
        base.CloseStore();
        _canvas.SetActive(false);
        
        print("close store");
    }

    private void SetLureInfo()
    {
        _activeLure = _lures[_observingLure];
        _lures[_observingLure].gameObject.SetActive(true);
        
        _name.text = _activeLure.LuresSO().name;

        if (_activeLure.LuresSO().Wires == 0 && _activeLure.LuresSO().Bolts == 0 && _activeLure.LuresSO().Huts == 0)
        {
            _activeLure.SetCreated(true);
        }
        
        if (_activeLure.IsCreated())
        {
            _chooseButton.SetActive(true);
            _craftButton.SetActive(false);
            _craftInfoPanel.SetActive(false);
            
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
