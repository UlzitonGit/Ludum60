using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private LocalizationManager _localization;
    public override void InstallBindings()
    {
        
        Container.Bind<LocalizationManager>().FromInstance(_localization).AsSingle();
        
        _localization.Init();
    }
}
