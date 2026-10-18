using UnityEngine;

public class Lure : MonoBehaviour
{
    [SerializeField] private LuresSO _luresSO;

    private bool isCreated;

    public bool IsCreated()
    {
        return isCreated;
    }

    public void SetCreated(bool create)
    {
        isCreated = create;
    }

    public LuresSO LuresSO()
    {
        return _luresSO;
    }
}
