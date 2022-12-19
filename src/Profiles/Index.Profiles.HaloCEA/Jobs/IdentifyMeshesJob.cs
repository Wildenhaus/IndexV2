using Assimp;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class IdentifyMeshesJob : JobBase
  {

    protected SceneContext Context { get; set; }

    #region Constructor

    public IdentifyMeshesJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    protected override async Task OnInitializing()
    {
      Context = Parameters.Get<SceneContext>();
    }

    protected override async Task OnExecuting()
    {
      SetIncludeUnitsInStatus();
      SetStatus( "Identifying Meshes" );
      SetCompletedUnits( 0 );
      SetTotalUnits( 2 );

      var lodMeshSet = CreateLodMeshSet();
      Parameters.Set( "LodMeshSet", lodMeshSet );
      IncreaseCompletedUnits( 1 );

      var volumeMeshSet = CreateVolumeMeshSet( Context.Scene );
      Parameters.Set( "VolumeMeshSet", volumeMeshSet );
      IncreaseCompletedUnits( 1 );
    }

    private HashSet<string> CreateLodMeshSet()
    {
      var set = new HashSet<string>();

      Data_02E4 templateData = null;
      if ( Parameters.TryGet<Template>( out var template ) )
        templateData = template.Data_02E4;
      else
        Parameters.TryGet<Data_02E4>( out templateData );

      if ( templateData is null )
        return set;

      var lodDefinitions = templateData.LodDefinitions;
      if ( lodDefinitions is null )
        return set;

      foreach ( var lodDefinition in lodDefinitions )
      {
        if ( lodDefinition.Index == 0 )
          continue;

        if ( Context.SkinCompoundIds.Contains( lodDefinition.ObjectId ) )
        {
          var lodDefinitionObjects = Context.Objects.Values
            .Where( x => x.SubmeshData != null )
            .Where( x => x.SubmeshData.SubmeshList.Any( y => y.SkinCompoundId == lodDefinition.ObjectId ) );

          foreach ( var obj in lodDefinitionObjects )
            set.Add( obj.ObjectInfo.Name );
        }
        else
        {
          var lodObject = templateData.Objects.FirstOrDefault( x => x.ObjectInfo.Id == lodDefinition.ObjectId );
          set.Add( lodObject.ObjectInfo.Name );
        }
      }

      return set;
    }

    private HashSet<string> CreateVolumeMeshSet( Scene scene )
    {
      var set = new HashSet<string>();
      foreach ( var obj in Context.Objects.Values )
        if ( obj.ObjectInfo.Flags[ 6 ] && ( obj.ObjectInfo.Flags[ 7 ] && obj.ObjectInfo.Flags[ 9 ] ) )
          set.Add( obj.ObjectInfo.Name );

      //EvaluateMesh( set, scene, scene.RootNode, IsVolumeMesh );

      return set;
    }

    private bool IsVolumeMesh( Scene scene, Node node )
    {
      if ( !node.HasMeshes )
        return false;

      if ( scene.MaterialCount == 1 && scene.Materials[ 0 ].Name == "DefaultMaterial" )
        return true;

      //var meshIndices = node.MeshIndices;
      //foreach ( var meshIndex in meshIndices )
      //{
      //  var mesh = scene.Meshes[ meshIndex ];
      //  var materialIndex = mesh.MaterialIndex;

      //  // This should only happen for Scene-Embedded templates
      //  //if ( materialIndex >= scene.MaterialCount )
      //  //  continue;

      //  if ( node.Name.Contains( "fog" ) )
      //    1.ToString();

      //  if ( materialIndex == 0 )
      //    return true;

      //  //var material = scene.Materials[ materialIndex ];
      //  //if ( material.Name.StartsWith( "sc_" ) )
      //  //  return true;
      //}

      return false;
    }

    private void EvaluateMesh( ISet<string> set, Scene scene, Node node, Func<Scene, Node, bool> evaluatorFunc )
    {
      var name = node.Name;
      if ( evaluatorFunc( scene, node ) )
        set.Add( name );

      foreach ( var child in node.EnumerateChildren() )
        EvaluateMesh( set, scene, child, evaluatorFunc );
    }


  }

}
