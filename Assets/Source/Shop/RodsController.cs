using UnityEngine;

public class RodsController : MonoBehaviour
{
    [SerializeField] private GameObject[] rods;
    private int index;

    public void ChangeRod()
    {
        rods[index].SetActive(false);
        index++;
        rods[index].SetActive(true);
    }
}
