using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.FileSystem;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.FileSystem.Files;
using Index.Profiles.SpaceMarine2.Meshes;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Serialization;
using LibSaber.SpaceMarine2.Structures;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
{

  public class DeserializeSceneJob : JobBase<SceneContext>
  {

    #region Properties

    private IFileSystem FileSystem { get; }

    #endregion

    #region Constructor

    public DeserializeSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      FileSystem = container.Resolve<IFileSystem>();
    }

    #endregion

    #region Overrides

    protected override async Task OnExecuting()
    {
      SetStatus( "Deserializing Scene" );
      SetIndeterminate();

      var assetReference = Parameters.Get<IAssetReference>();

      var lgFile = GetLgFile( assetReference );
      var lgDataFile = GetLgDataFile( assetReference );

      using var stream = lgFile.Open();
      var reader = new NativeReader(stream, Endianness.LittleEndian);

      var name = Path.GetFileNameWithoutExtension( assetReference.AssetName );
      var scene = Serializer<scnSCENE>.Deserialize( reader );

      // NOTE: For some reason the lg_data files have a header, so the offsets are borked.
      // Use a StreamSegment to pretend the header isn't there
      var lgDataStream = lgDataFile.Open();
      var lgDataViewStream = new StreamSegment( lgDataStream, 0x10, lgDataStream.Length - 0x10 );
      var context = new SceneContext( name, scene.GeometryGraph, lgDataViewStream );
      var textures = new Dictionary<string, ITextureAsset>();

      Parameters.Set( context );
      Parameters.Set( scene );
      Parameters.Set( "Textures", textures );

      SetResult( context );
    }

    private IFileSystemNode GetLgFile( IAssetReference assetReference )
    {
      var assetNode = assetReference.Node as SM2SceneResourceFileNode;
      ASSERT( assetNode is not null, "Scene AssetNode is null." );

      var lgFile = assetNode.ResourceDescription.lg;
      var lgFileNode = FileSystem.EnumerateFiles()
        .SingleOrDefault( x => Path.GetFileName( x.Name ) == lgFile );

      return lgFileNode;
    }

    private IFileSystemNode GetLgDataFile( IAssetReference assetReference )
    {
      var assetNode = assetReference.Node as SM2SceneResourceFileNode;
      ASSERT( assetNode is not null, "Scene AssetNode is null." );

      var lgDataFile = assetNode.ResourceDescription.lgData;
      var lgDataFileNode = FileSystem.EnumerateFiles()
        .SingleOrDefault( x => Path.GetFileName( x.Name ) == lgDataFile );

      return lgDataFileNode;
    }

    #endregion


  }

}
