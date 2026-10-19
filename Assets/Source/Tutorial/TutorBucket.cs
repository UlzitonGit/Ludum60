using System;
using UnityEngine;
using Zenject;

public class TutorBucket : MonoBehaviour, IStage
{
    [SerializeField] private Fish _fish;
    [SerializeField] private FishArea _fishArea;
    private FishBucket _fishBucket;
    
    private BucketUIController _bucketUIController;

    private bool setted = false;
    private Hook _hook;

    [Inject]
    public void Construct(FishBucket fishBucket, Hook hook)
    {
        _fishBucket = fishBucket;
        _hook = hook;
        
        setted = true;
    }
    

    public void StartStage()
    {

        Fish spawnedFish = _fishArea.SpawnFish(_fish);
        
        _hook.SetFish(spawnedFish);


        _bucketUIController = _fishBucket.GetBucketUIController();
        
        _bucketUIController.AddFish(true);
        
        _fish.SetIsInWater(true);
    }

    public void EndStage()
    {
        setted = false;
    }
}
