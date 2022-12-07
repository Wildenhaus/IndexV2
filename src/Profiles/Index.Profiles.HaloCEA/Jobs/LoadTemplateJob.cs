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

  public class LoadTemplateJob : LoadGeometryJobBase<CEATemplateAsset>
  {

    #region Properties

    protected SceneContext Context { get; set; }
    protected Template Template { get; set; }

    #endregion

    #region Constructor

    public LoadTemplateJob( IContainerProvider container, IParameterCollection parameters )
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

        Template = Template.Deserialize( reader, new SerializationContext() );

        var context = SceneContext.Create( Template.Data_02E4.Objects );

        return context;
      } );
    }

    protected override IList<TextureListEntry> GetTextureList()
      => Template.Data_02E4.TextureList;

    protected override CEATemplateAsset CreateAsset( Scene assimpScene )
    {
      var asset = new CEATemplateAsset( AssetReference );
      asset.AssimpScene = assimpScene;

      return asset;
    }

    #endregion

  }

}
