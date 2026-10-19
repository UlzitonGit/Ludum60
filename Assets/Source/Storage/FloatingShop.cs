using System.Resources;
using UnityEngine;
using Zenject;

public class FloatingShop : InteractiveTable
{
    private ResourcesManager _resourceManager;

    [SerializeField] private GameObject[] upgrades;
    
    [SerializeField] private Animator animator;

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
        base.OpenStore(click);
        if (!inZone)
        {
            CloseStore();
            return;
        }
      
        animator.SetFloat("Speed", 0);
        if (_upgrades < upgrades.Length)
        {
            upgrades[_upgrades].SetActive(true);
        }
    }

    public void Setup()
    {
        animator.SetTrigger("End");
        CloseStore();
        
    }

    public override void CloseStore()
    {
        base.CloseStore(); 
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
            }
                
            shopItem.Purchase();
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
            
            CloseStore();
        }
    }
}
