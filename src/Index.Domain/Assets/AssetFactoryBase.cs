using Index.Jobs;
using Prism.Ioc;

namespace Index.Domain.Assets
{

  public abstract class AssetFactoryBase<TAsset> : IAssetFactory<TAsset>
    where TAsset : class, IAsset
  {

    #region Properties

    protected IAssetManager AssetManager { get; }
    protected IJobManager JobManager { get; }

    #endregion

    #region Constructor

    protected AssetFactoryBase( IContainerProvider container )
    {
      AssetManager = container.Resolve<IAssetManager>();
      JobManager = container.Resolve<IJobManager>();
    }

    #endregion

    #region Public Methods

    public abstract IJob<TAsset> LoadAsset( IAssetReference assetReference );

    #endregion

  }

}
