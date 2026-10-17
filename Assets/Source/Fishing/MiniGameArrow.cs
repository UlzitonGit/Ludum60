using System;
using UnityEngine;

public class MiniGameArrow : MonoBehaviour
{
    private bool _onZone;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MinigameZone"))
        {
            _onZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MinigameZone"))
        {
            _onZone = false;
        }
    }

    public bool OnZone()
    {
        return _onZone;
    }
}
