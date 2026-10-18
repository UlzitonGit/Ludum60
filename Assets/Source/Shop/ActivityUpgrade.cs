using UnityEngine;

public class ActivityUpgrade : ShopItem
{
    public override void Purchase()
    {
        heart.SetFishActivity(2);
    }
}
