using Infrastructure.Services.AssetsProvider;
using Infrastructure.Services.AutoPlayControl;
using Infrastructure.Services.ConfigLoad;
using Infrastructure.Services.PrefabLoad;
using Infrastructure.Services.ProgressLoad;
using Infrastructure.Services.UpgradeRegistry;
using Zenject;

namespace Infrastructure.Installers.BootstrapInstaller
{
    public class ServiceInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            IPrefabLoader prefabLoader = 
                InstallPrefabLoader();
            
            IConfigProvider configProvider = 
                InstallConfigLoader(prefabLoader);

            IAssetProvider assetProvider = 
                InstallAssetProvider(prefabLoader);

            IProgressProvider progressProvider = 
                InstallProgressProvider(prefabLoader);

            IAutoPlayController autoPlayController = 
                InstallAutoProcessesController();
        }

        #region Installs

        private IAutoPlayController InstallAutoProcessesController()
        {
            IAutoPlayController autoPlayController = new AutoPlayController();
            Container.Bind<IAutoPlayController>().FromInstance(autoPlayController).AsSingle();
            return autoPlayController;
        }

        private IProgressProvider InstallProgressProvider(IPrefabLoader prefabLoader)
        {
            IProgressProvider progressProvider = new ProgressProvider(prefabLoader);
            Container.Bind<IProgressProvider>().FromInstance(progressProvider).AsSingle();
            return progressProvider;
        }

        private IAssetProvider InstallAssetProvider(IPrefabLoader prefabLoader)
        {
            IAssetProvider assetProvider = new AssetProvider(prefabLoader, Container);
            Container.Bind<IAssetProvider>().FromInstance(assetProvider).AsSingle();
            return assetProvider;
        }

        private IConfigProvider InstallConfigLoader(IPrefabLoader prefabLoader)
        {
            IConfigProvider configProvider = new ConfigProvider(prefabLoader);
            Container.Bind<IConfigProvider>().FromInstance(configProvider).AsSingle();
            return configProvider;
        }

        private IPrefabLoader InstallPrefabLoader()
        {
            IPrefabLoader prefabLoader = new PrefabLoader();
            Container.Bind<IPrefabLoader>().FromInstance(prefabLoader).AsSingle();
            return prefabLoader;
        }
        
        #endregion
    }
}