using UnityEngine;
using Zenject;

public class BloodSuply : ShopItem
{
    public override void Purchase()
    {
        heart.AddFee(60);
    }
}
