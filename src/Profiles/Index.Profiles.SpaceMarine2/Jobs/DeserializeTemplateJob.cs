using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.FileSystem;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.FileSystem.Files;
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

    #region Properties

    private IFileSystem FileSystem { get; }

    #endregion

    #region Constructor

    public DeserializeTemplateJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      FileSystem = container.Resolve<IFileSystem>();
    }

    #endregion

    #region Overrides

    protected override async Task OnExecuting()
    {
      SetStatus( "Deserializing Template" );
      SetIndeterminate();

      var assetReference = Parameters.Get<IAssetReference>();

      var tplFile = GetTplFile( assetReference );
      var tplDataFile = GetTplDataFile( assetReference );

      using var stream = tplFile.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      var name = Path.GetFileNameWithoutExtension( assetReference.AssetName );
      var template = Serializer<animTPL>.Deserialize( reader, new SerializationContext() );

      var tplDataStream = tplDataFile.Open();
      var context = new SceneContext( name, template.GeometryGraph, tplDataStream );
      //context.AddLodDefinitions( template.LodDefinitions );

      var textures = new Dictionary<string, ITextureAsset>();


      Parameters.Set( context );
      Parameters.Set( template );
      Parameters.Set( "Textures", textures );

      SetResult( context );
    }

    private IFileSystemNode GetTplFile( IAssetReference assetReference )
    {
      var assetNode = assetReference.Node as SM2TemplateResourceFileNode;
      ASSERT( assetNode is not null, "Template AssetNode is null." );

      var tplFile = assetNode.ResourceDescription.tpl;
      var tplFileNode = FileSystem.EnumerateFiles().SingleOrDefault(x => Path.GetFileName(x.Name) == tplFile );

      return tplFileNode;
    }

    private IFileSystemNode GetTplDataFile( IAssetReference assetReference )
    {
      var assetNode = assetReference.Node as SM2TemplateResourceFileNode;
      ASSERT( assetNode is not null, "Template AssetNode is null." );

      var tplDataFile = assetNode.ResourceDescription.tplData;
      ASSERT( !string.IsNullOrWhiteSpace( tplDataFile ), "Template does not have an associated .tpl_data file." );

      var tplDataFileNode = FileSystem.EnumerateFiles().SingleOrDefault( x => Path.GetFileName( x.Name ) == tplDataFile );
      ASSERT( tplDataFileNode is not null, "TPL_DATA file not found." );

      return tplDataFileNode;
    }

    #endregion

  }

}
