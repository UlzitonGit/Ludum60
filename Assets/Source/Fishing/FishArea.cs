using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class FishArea : MonoBehaviour
{
    
    [SerializeField] private FishesSO fishesSo;
    
    [SerializeField] private float[] durationBorders;
    
    [SerializeField] private FishingLine fishingLine;
    
    private LuresSO activeLure;
    
    private Hook hook;
    
    private MainHeart _heart;
    
    private FishingMinigame minigame;
    
    [Inject]
        public void Construct(FishingMinigame minigame, Hook hook, MainHeart heart)
    {
        _heart = heart;
        this.minigame = minigame;
        this.hook = hook;
    }
    
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

    public void SetActiveLure(LuresSO lure)
    {
        activeLure = lure;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(durationBorders[0] / _heart.GetFishActivity(), durationBorders[1] / _heart.GetFishActivity()));

        List<Fish> fishesToCatch = new List<Fish>();
        
        foreach (var fishes in fishesSo.fishes)
        {
            if (activeLure.fishesToCatch.Contains(fishes))
            {
                fishesToCatch.Add(fishes);
            }
        }
        
        int chance = Random.Range(0, 100);

        int i = 0;

        foreach (int chanceC in activeLure.catchChance)
        {
            if (chance < chanceC)
                break;
            i++;
        }
        
        Fish fish = activeLure.fishesToCatch[i];
        
        Fish spawnedFish = Instantiate(fish, hook.transform.position, Quaternion.identity).GetComponent<Fish>();
        
        spawnedFish.Initialize(hook, fishingLine, minigame);
    }
}
