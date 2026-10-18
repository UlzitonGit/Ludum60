using UnityEngine;
using Zenject;

public class OxygenMask : ShopItem
{
    
    public override void Purchase()
    {
        heart.AddOxygen(60);
    }
}
