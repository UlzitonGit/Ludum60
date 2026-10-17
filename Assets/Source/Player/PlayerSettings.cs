using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SO/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] public float MovementSpeed = 1.0f;
    [SerializeField] public float RotationSpeed = 1.0f;

    [SerializeField] public float MinPitch;
    [SerializeField] public float MaxPitch;
    [SerializeField] public float LookSmoothing;

}