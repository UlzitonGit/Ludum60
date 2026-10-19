using UnityEngine;

public class ActivityUpgrade : ShopItem
{
    [SerializeField] private RodsController controller;
    public override void Purchase()
    {
        heart.SetFishActivity(2);
        controller.ChangeRod();
    }
    public override void TryBuy()
    {
        _shop.TryBuy(_wires, _nuts, _bolts, this, true);
    }
}
