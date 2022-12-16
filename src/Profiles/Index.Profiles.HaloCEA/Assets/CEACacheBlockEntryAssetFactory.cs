using Index.Domain.Assets;
using Index.Domain.Jobs;
using Index.Jobs;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEACacheBlockEntryAssetFactory<TAsset> : AssetFactoryBase<TAsset>
    where TAsset : CEACacheBlockEntryAsset
  {

    #region Constructor

    public CEACacheBlockEntryAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<TAsset> LoadAsset( IAssetReference assetReference )
    {
      var asset = CreateAsset( assetReference );
      asset.TextStream = assetReference.Node.Open();

      return CompletedJob.FromResult( asset );
    }

    #endregion

    #region Private Methods

    private TAsset CreateAsset( IAssetReference assetReference )
      => ( TAsset ) Activator.CreateInstance( typeof( TAsset ), new object[] { assetReference } );

    #endregion

  }

}
