using Assimp;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class IdentifyMeshesJob : JobBase
  {

    private static readonly string[] VOLUME_NAME_HINTS = new string[]
    {
      ".sh_cast",
      "sh_cast",
      "cin_",
      "start1",
      ".pCube",
      "path_",
      "add_",
      ".pPlane",

    };

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

      var lodMeshSet = CreateLodMeshSet( Context.Scene );
      Parameters.Set( "LodMeshSet", lodMeshSet );
      IncreaseCompletedUnits( 1 );

      var volumeMeshSet = CreateVolumeMeshSet( Context.Scene );
      Parameters.Set( "VolumeMeshSet", volumeMeshSet );
      IncreaseCompletedUnits( 1 );
    }

    private HashSet<string> CreateLodMeshSet( Scene scene )
    {
      var set = new HashSet<string>();
      EvaluateMesh( set, scene.RootNode, IsLodMesh );

      return set;
    }

    private HashSet<string> CreateVolumeMeshSet( Scene scene )
    {
      var set = new HashSet<string>();
      EvaluateMesh( set, scene.RootNode, IsVolumeMesh );

      return set;
    }

    private bool IsLodMesh( string meshName )
      => meshName.Contains( ".lod." );

    private bool IsVolumeMesh( string meshName )
    {
      foreach ( var hint in VOLUME_NAME_HINTS )
        if ( meshName.StartsWith( hint, StringComparison.InvariantCultureIgnoreCase ) )
          return true;

      return false;
    }

    private void EvaluateMesh( ISet<string> set, Node node, Func<string, bool> evaluatorFunc )
    {
      var name = node.Name;
      if ( evaluatorFunc( name ) )
        set.Add( name );

      foreach ( var child in node.EnumerateChildren() )
        EvaluateMesh( set, child, evaluatorFunc );
    }


  }

}
