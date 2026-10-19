using UnityEngine;

public class DecreaseUpgrade : ShopItem
{
    public override void Purchase()
    {
        heart.SetDecreaseSpeed(0);
    }

    public override void TryBuy()
    {
        _shop.TryBuy(_wires, _nuts, _bolts, this, true);
    }
}
