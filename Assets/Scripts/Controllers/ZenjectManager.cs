using UnityEngine;
using Zenject;

public class ZenjectManager : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Model>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Space>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
    }
}
