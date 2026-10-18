using UnityEngine;

public class DecreaseUpgrade : ShopItem
{
    public override void Purchase()
    {
        heart.SetDecreaseSpeed(0);
    }
}
