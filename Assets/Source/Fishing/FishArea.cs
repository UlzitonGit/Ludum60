using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishArea : MonoBehaviour
{
    [SerializeField] private Hook hook;
    
    [SerializeField] private FishesSO fishesSo;
    
    [SerializeField] private float[] durationBorders;
    
    [SerializeField] private FishingLine fishingLine;
    
    [SerializeField] private FishingMinigame minigame;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            StartCoroutine(Delay());
            print(hook);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(durationBorders[0], durationBorders[1]));

        Fish fish = fishesSo.fishes[Random.Range(0, fishesSo.fishes.Length)];
        
        Fish spawnedFish = Instantiate(fish, hook.transform.position, Quaternion.identity).GetComponent<Fish>();
        
        spawnedFish.Initialize(hook, fishingLine, minigame);
    }
}
