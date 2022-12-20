using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.Halo2A.Meshes;
using LibSaber.Halo2A.IO;
using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Jobs
{

  public class DeserializeSceneJob : JobBase<SceneContext>
  {

    #region Constructor

    public DeserializeSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override async Task OnExecuting()
    {
      SetStatus( "Deserializing Scene" );
      SetIndeterminate();

      var assetReference = Parameters.Get<IAssetReference>();
      var stream = assetReference.Node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      var name = Path.GetFileNameWithoutExtension( assetReference.AssetName );
      var scene = Serializer.Deserialize<SaberScene>( reader, new SerializationContext() );
      var context = new SceneContext( name, scene.GeometryGraph, stream );
      var textures = new Dictionary<string, ITextureAsset>();

      Parameters.Set( context );
      Parameters.Set( scene );
      Parameters.Set( "Textures", textures );

      SetResult( context );
    }

    #endregion

  }

}
