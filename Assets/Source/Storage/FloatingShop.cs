using System.Resources;
using UnityEngine;
using Zenject;

public class FloatingShop : InteractiveTable
{
    private ResourcesManager _resourceManager;

    [SerializeField] private GameObject[] upgrades;
    
    [SerializeField] private Animator animator;

    private int _upgrades;

    [Inject]
    public void Construct(ResourcesManager resourceManager)
    {
        _resourceManager = resourceManager;
        animator.SetFloat("Speed", 1);
    }

    protected override void OpenStore(bool click)
    {
        base.OpenStore(click);
        if (inZone)
        {
            animator.SetFloat("Speed", 0);
        }
        if (_upgrades < upgrades.Length)
        {
            upgrades[_upgrades].SetActive(true);
        }
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
}
