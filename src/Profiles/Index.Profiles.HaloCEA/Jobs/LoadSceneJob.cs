using Assimp;
using Index.Domain.Assets;
using Index.Profiles.HaloCEA.Assets;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadSceneJob : LoadGeometryJobBase<CEASceneAsset>
  {

    #region Properties

    protected SaberScene Scene { get; set; }

    #endregion

    #region Constructor

    public LoadSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override Task<SceneContext> CreateSceneContext( IAssetReference assetReference )
    {
      return Task.Run( () =>
      {
        var stream = assetReference.Node.Open();
        var reader = new NativeReader( stream, Endianness.LittleEndian );

        Scene = SaberScene.Deserialize( reader, new SerializationContext() );

        var context = SceneContext.Create( Scene.Objects );

        return context;
      } );
    }

    protected override IList<TextureListEntry> GetTextureList()
      => Scene.TextureList;

    protected override CEASceneAsset CreateAsset( Scene assimpScene )
    {
      var asset = new CEASceneAsset( AssetReference );
      asset.AssimpScene = assimpScene;
      asset.Textures = Textures;

      return asset;
    }

    #endregion

  }

}
