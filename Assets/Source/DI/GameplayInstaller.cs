using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InputHandler inputHandler = new InputHandler();
        
        Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle();
    }
}
