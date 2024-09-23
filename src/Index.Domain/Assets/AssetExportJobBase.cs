using Index.Jobs;
using Prism.Ioc;

namespace Index.Domain.Assets
{

  public abstract class AssetExportJobBase<TAsset, TOptions> : JobBase
    where TAsset : class, IAsset
    where TOptions : AssetExportOptions
  {

    #region Properties

    protected IAssetManager AssetManager { get; }

    protected TAsset Asset { get; private set; }
    protected IAssetReference AssetReference { get; }

    protected TOptions Options { get; }

    #endregion

    #region Constructor

    public AssetExportJobBase( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();

      if ( Parameters.TryGet<TAsset>( "Asset", out var asset ) )
        Asset = asset;
      AssetReference = Parameters.Get<IAssetReference>( "AssetReference" );

      Options = Parameters.Get<TOptions>( "Options" );

      Name = $"Exporting {AssetReference.AssetName}";
    }

    #endregion

    #region Overrides

    protected sealed override async Task OnInitializing()
    {
      if ( Asset is not null )
        return;

      SetStatus( "Loading Asset" );
      var loadAssetJob = AssetManager.LoadAsset<TAsset>( AssetReference );
      await loadAssetJob.Completion;

      Asset = loadAssetJob.Result;
    }

    protected sealed override async Task OnExecuting()
    {
      SetStatus( "Exporting {0}", Asset.AssetName );
      await ExportAsset();
      await OnExportAdditionalData();
    }

    #endregion

    #region Private Methods

    protected abstract Task ExportAsset();

    protected virtual Task OnExportAdditionalData() => Task.CompletedTask;

    #endregion

  }

}
