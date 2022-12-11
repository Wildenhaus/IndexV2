using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Assets;
using Index.Jobs;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class DeserializeSceneJob : JobBase<SceneContext>
  {

    public DeserializeSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    protected override Task OnExecuting()
    {
      return Task.Run( () =>
      {
        SetStatus( "Deserializing Scene" );
        SetIndeterminate();

        var assetReference = Parameters.Get<IAssetReference>();
        var stream = assetReference.Node.Open();
        var reader = new NativeReader( stream, Endianness.LittleEndian );

        var scene = SaberScene.Deserialize( reader, new SerializationContext() );
        var context = SceneContext.Create( scene.Objects );

        Parameters.Set( context );
        Parameters.Set( scene );
        Parameters.Set( scene.TextureList );

        SetResult( context );
      } );
    }

  }

}
