using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SO/Lure")]

public class LuresSO : ScriptableObject
{
    [SerializeField] public int Wires;
    [SerializeField] public int Huts;
    [SerializeField] public int Bolts;

    [SerializeField] public string type;
    [SerializeField] public string name;

    [SerializeField] public Fish[] fishesToCatch;
    [SerializeField] public float[] catchChance;
}
