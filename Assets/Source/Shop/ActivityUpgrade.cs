using UnityEngine;

public class ActivityUpgrade : ShopItem
{
    public override void Purchase()
    {
        heart.SetFishActivity(2);
    }
    public override void TryBuy()
    {
        _shop.TryBuy(_wires, _nuts, _bolts, this, true);
    }
}
