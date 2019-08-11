using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    public override void InstallBindings() {
        // Neccessary to use IInitializable and others
        Container.BindInterfacesAndSelfTo<LocalInputProvider>()
            .AsSingle();

        Container.Bind<IGameSettingsProvider>()
            .To<DefaultGameSettingsProvider>()
            .AsSingle();
    }
}