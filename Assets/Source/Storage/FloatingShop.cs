using System.Resources;
using UnityEngine;
using Zenject;

public class FloatingShop : InteractiveTable
{
    private ResourcesManager _resourceManager;

    [SerializeField] private GameObject[] upgrades;

    private int _upgrades;

    [Inject]
    public void Construct(ResourcesManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    protected override void OpenStore(bool click)
    {
        base.OpenStore(click);
        if (_upgrades < upgrades.Length)
        {
            upgrades[_upgrades].SetActive(true);
        }
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
