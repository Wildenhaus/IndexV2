using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.FileSystem;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Meshes;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.SpaceMarine2.Serialization;
using LibSaber.SpaceMarine2.Structures;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
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
      var template = Serializer<animTPL>.Deserialize( reader, new SerializationContext() );

      var tplDataFile = GetTplDataFile( assetReference );
      var tplDataStream = tplDataFile.Open();

      var context = new SceneContext( name, template.GeometryGraph, tplDataStream );
      //context.AddLodDefinitions( template.LodDefinitions );

      var textures = new Dictionary<string, ITextureAsset>();


      Parameters.Set( context );
      Parameters.Set( template );
      Parameters.Set( "Textures", textures );

      SetResult( context );
    }

    private IFileSystemNode GetTplDataFile( IAssetReference assetReference )
    {
      var fileName = assetReference.Node.Name;
      fileName += "_data";

      return assetReference.Node.Device.EnumerateFiles().Where( x => x.Name == fileName ).FirstOrDefault();
    }

    #endregion

  }

}
