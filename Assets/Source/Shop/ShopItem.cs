using TMPro;
using UnityEngine;
using Zenject;

public abstract class ShopItem : MonoBehaviour, IShopItem
{
    [SerializeField] protected FloatingShop _shop;
    
    [SerializeField] private TextMeshProUGUI _wireText;
    [SerializeField] private TextMeshProUGUI _nutsText;
    [SerializeField] private TextMeshProUGUI _boltsText;

    [SerializeField] private int _wires;
    [SerializeField] private int _nuts;
    [SerializeField] private int _bolts;
    
    protected MainHeart heart;

    [Inject]
    public void Construct(MainHeart heart)
    {
        this.heart = heart;
        
        _wireText.text = _wires.ToString();
        _nutsText.text = _nuts.ToString();
        _boltsText.text = _bolts.ToString();
    }

    public virtual void TryBuy()
    {
        _shop.TryBuy(_wires, _nuts, _bolts, this);
    }

    public virtual void Purchase()
    {
      
    }
}
