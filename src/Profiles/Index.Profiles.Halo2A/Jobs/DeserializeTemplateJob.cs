using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.Halo2A.Meshes;
using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Jobs
{

  public class DeserializeTemplateJob : JobBase<SceneContext>
  {

    #region Constructor

    public DeserializeTemplateJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override async Task OnExecuting()
    {
      SetStatus( "Deserializing Template" );
      SetIndeterminate();

      var assetReference = Parameters.Get<IAssetReference>();
      var stream = assetReference.Node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      var name = Path.GetFileNameWithoutExtension( assetReference.AssetName );
      var template = Serializer.Deserialize<Template>( reader, new SerializationContext() );

      var context = new SceneContext( name, template.GeometryGraph, stream );
      context.AddLodDefinitions( template.LodDefinitions );

      var textures = new Dictionary<string, ITextureAsset>();

      Parameters.Set( context );
      Parameters.Set( template );
      Parameters.Set( "Textures", textures );

      SetResult( context );
    }

    #endregion

  }

}
