using Managers;
using Storage;
using StorageService;
using ResourceService;
using UI.Shop.Data;
using Zenject;

public class ProjectMonoInstallers : MonoInstaller
{
    public override void InstallBindings()
    {
        GoogleSheetLoading();
        System();
        Loading();
        Data();
        Storage();
        Shop();
    }

    private void System()
    {
        Container.Bind<WindowManager>().AsSingle();
        Container.Bind<AssetManager>().AsSingle();
        var tryAmount = 3;
        var waitingDelay = 3000;
        Container.Bind<ConnectionManager>().AsSingle().WithArguments(tryAmount, waitingDelay);
    }

    private void GoogleSheetLoading()
    {
        Container.Bind<GoogleSheetLoader>()
            .AsSingle()
            .WithArguments("1b5Ak77i6ubJFIcFagXtlwf2mwrYZrXJ3qOPp5c85NgQ", false);
    }

    private void Loading()
    {
        Container.BindInterfacesTo<DataLoader>().AsSingle();
        Container.BindInterfacesTo<GameSceneLoader>().AsTransient();
        Container.Bind<LoadingScreenLoader>().AsSingle();
    }

    private void Data()
    {
        Container.Bind<DataManager>().AsSingle();
        Container.Bind<Wallet>().ToSelf().AsSingle();
    }

    private void Shop()
    {
        Container.Bind<ShopData>().AsSingle();
    }

    private void Storage()
    {
        Container.Bind<string>().WithId("Server").FromInstance("http://game.ispu.ru/insight");
        Container.BindInterfacesTo<ServerJSONStorageService>().AsSingle();
        Container.BindInterfacesTo<ServerStorageService>().AsSingle();
        Container.Bind<ResourceManager>().AsSingle().NonLazy();
        Container.Bind<LevelManager>().AsSingle().Lazy();
    }
}