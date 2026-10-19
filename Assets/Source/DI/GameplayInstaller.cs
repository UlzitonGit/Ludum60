using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
     [SerializeField] private Hook _hook;
     
     [SerializeField] private FishingMinigame _minigame;
     
     [SerializeField] private FishingRodController _fishingRodController;
     
     [SerializeField] private FishBucket _fishBucket;
     
     [SerializeField] private MainHeart _heart;
    
    
    public override void InstallBindings()
    {
        InputHandler inputHandler = new InputHandler(_fishingRodController);
        
        ResourcesManager resourcesManager = new ResourcesManager();
        
        Container.Bind<ResourcesManager>().FromInstance(resourcesManager).AsSingle();
        
        Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle();
        
        Container.Bind<Hook>().FromInstance(_hook).AsSingle();
        
        Container.Bind<FishingMinigame>().FromInstance(_minigame).AsSingle();
        
        Container.Bind<FishingRodController>().FromInstance(_fishingRodController).AsSingle();
        
        Container.Bind<FishBucket>().FromInstance(_fishBucket).AsSingle();
        
        Container.Bind<MainHeart>().FromInstance(_heart).AsSingle();
    }
}
