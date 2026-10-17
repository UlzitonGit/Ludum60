using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SO/FishesSO")]
public class FishesSO : ScriptableObject
{
    [SerializeField] public Fish[] fishes;
    
}
