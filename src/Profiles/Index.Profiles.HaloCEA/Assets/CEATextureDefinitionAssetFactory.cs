using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATextureDefinitionAssetFactory : CEACacheBlockEntryAssetFactory<CEATextureDefinitionAsset>
  {

    #region Constructor

    public CEATextureDefinitionAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

  }

}
