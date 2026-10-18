using UnityEngine;

public class RodUpgrade : ShopItem
{
    public override void Purchase()
    {
        heart.SetRodPower(2);
    }
}
