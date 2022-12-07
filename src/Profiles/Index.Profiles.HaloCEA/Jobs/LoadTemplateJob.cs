using Assimp;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadTemplateJob : JobBase<MeshAsset>
  {

    #region Data Members

    private IAssetReference _assetReference;
    private Data_02E4 _template;

    #endregion

    #region Constructor

    public LoadTemplateJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      _assetReference = Parameters.Get<IAssetReference>();
      Name = $"Loading {_assetReference.AssetName}";
    }

    #endregion

    #region Overrides

    protected override Task OnInitializing()
    {
      return Task.Run( () =>
      {
        SetStatus( "Deserializing Data" );
        SetIndeterminate();

        _template = DeserializeData( _assetReference );
      } );
    }

    protected override Task OnExecuting()
    {
      return Task.Run( () =>
      {
        var scene = CreateScene();

        var asset = new MeshAsset( _assetReference ) { AssimpScene = scene };
        SetResult( asset );
      } );
    }

    #endregion

    #region Private Methods

    private Data_02E4 DeserializeData( IAssetReference assetReference )
    {
      var stream = assetReference.Node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      var template = Template.Deserialize( reader, new SerializationContext() );
      return template.Data_02E4;
    }

    private Scene CreateScene()
    {
      return new Scene();
    }

    #endregion

  }

}
