using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class FloatingShop : InteractiveTable
{
    private ResourcesManager _resourceManager;

    [SerializeField] private GameObject[] upgrades;
    
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject _knife;

    [SerializeField] private BoxCollider _boxCollider;


    private bool _hasKnife;
    
    private int _upgrades;
    
    private TutorialController _tutorialController;

    [Inject]
    public void Construct(ResourcesManager resourceManager, TutorialController tutorialController)
    {
        _resourceManager = resourceManager;
        _tutorialController = tutorialController;
        animator.SetFloat("Speed", 1);
    }

    protected override void OpenStore(bool click)
    {
        print(inZone);
        
        if(_tutorialController.IsTutorial() || !inZone) return;
        

        
        if (!inZone || !_boxCollider.enabled)
        {
            CloseStore();
            return;
        }
        base.OpenStore(click);
        
      
        animator.SetFloat("Speed", 0);
        if (_upgrades < upgrades.Length)
        {
            upgrades[_upgrades].SetActive(true);
        }
        _hint.SetActive(false);
    }

    public void Setup()
    {
        animator.SetTrigger("End");
        inZone = false;
        _hint.SetActive(false);
        CloseStore();
        
    }

    public override void CloseStore()
    {
        base.CloseStore(); 
        _hint.SetActive(false);
        inZone = false;
        animator.SetFloat("Speed", 1);
    }

    public void TryBuy(int wires, int nuts, int bolts, IShopItem shopItem, bool isUpgrade = false)
    {
        if (_resourceManager.GetBolts() >= bolts && _resourceManager.GetNuts() >= nuts &&
            _resourceManager.GetWires() >= wires)
        {
            _resourceManager.AddBolts(bolts * -1);
            _resourceManager.AddNuts(nuts * -1);
            _resourceManager.AddWires(wires * -1);

            if (isUpgrade)
            {
                upgrades[_upgrades].SetActive(false);
                _upgrades++;
                if (_upgrades < upgrades.Length)
                {
                    upgrades[_upgrades].SetActive(true);
                }

                if (_upgrades == 4)
                {
                    _knife.SetActive(true);
                    _hasKnife = true;
                }
            }
                
            shopItem.Purchase();
        }
    }

    public bool GetHasKnife()
    {
        return _hasKnife;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            if(!_tutorialController.IsTutorial())
                _hint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            _hint.SetActive(false);
            CloseStore();
        }
    }
}
