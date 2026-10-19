using UnityEngine;

public class RodUpgrade : ShopItem
{
    public override void Purchase()
    {
        heart.SetRodPower(2);
    }
    public override void TryBuy()
        {
            _shop.TryBuy(_wires, _nuts, _bolts, this, true);
        }
}
