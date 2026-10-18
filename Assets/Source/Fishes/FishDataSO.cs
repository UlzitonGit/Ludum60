using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SO/FishDataSO")]
public class FishDataSO : ScriptableObject
{
    [SerializeField] public float minWeight;
    [SerializeField] public float maxWeight;
    
    [SerializeField] public string name;
    [SerializeField] public string description;
    
    [SerializeField] public Sprite sprite;

    [SerializeField] public int bloodLevel;
    [SerializeField] public int oxygenLevel;
    
    [SerializeField] public float _forwardForce;
    
    [SerializeField] public float _backwardForce;

    [SerializeField] public int nuts;

    [SerializeField] public int bolts;

    [SerializeField] public int wires;
}
