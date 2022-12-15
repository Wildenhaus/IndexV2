using Index.Domain.Assets;
using Index.Jobs;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class DeserializeTemplateJob : JobBase<SceneContext>
  {

    public DeserializeTemplateJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    protected override Task OnExecuting()
    {
      return Task.Run( () =>
      {
        SetStatus( "Deserializing Template" );
        SetIndeterminate();

        var assetReference = Parameters.Get<IAssetReference>();
        var stream = assetReference.Node.Open();
        var reader = new NativeReader( stream, Endianness.LittleEndian );

        var template = Template.Deserialize( reader, new SerializationContext() );
        var context = SceneContext.Create( template.Data_02E4.Objects );

        var inverseMatrixData = template.Data_02E4.Sentinel_0305;
        if ( inverseMatrixData.HasValue )
          context.InverseMatrices = inverseMatrixData.Value.Sentinel_030D_02;

        Parameters.Set( context );
        Parameters.Set( template );
        Parameters.Set( template.Data_02E4.TextureList );

        SetResult( context );
      } );
    }

  }

}
