using Assimp;
using HelixToolkit.SharpDX.Core.Model.Scene;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Common;
using Index.Profiles.SpaceMarine2.Meshes;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
{

  public class IdentifyMeshesJob : JobBase
  {

    #region Data Members

    private static readonly HashSet<string> VolumeAffixes = new HashSet<string>
    {
      "visibility_hidden",
      "visibility_occluder",
      "lighting_sm_invisible_shadowcaster_affix"
    };

    #endregion

    #region Properties

    private SceneContext Context { get; set; }

    #endregion

    #region Constructor

    public IdentifyMeshesJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      Context = Parameters.Get<SceneContext>();
    }

    protected override async Task OnExecuting()
    {
      var lodSet = CreateLodMeshSet( Context.Scene );
      Parameters.Set( "LodMeshSet", lodSet );

      var volumeSet = CreateVolumeMeshSet( Context.Scene );
      Parameters.Set( "VolumeMeshSet", volumeSet );
    }

    #endregion

    #region Private Methods

    private HashSet<string> CreateLodMeshSet( Scene scene )
    {
      var set = new HashSet<string>();
      //EvaluateMesh( set, scene, scene.RootNode, IsVolumeMesh );

      foreach ( var pair in Context.LodIndices )
      {
        var objId = pair.Key;
        var lodIndex = pair.Value;

        if ( lodIndex == 0 )
          continue;

        if ( Context.SkinCompounds.ContainsKey( objId ) )
        {
          var skinCompoundObjects = Context.GeometryGraph.objects
            .Where( x => x.SubMeshes.Any( y => y.BufferInfo.SkinCompoundId == objId ) );

          foreach ( var obj in skinCompoundObjects )
            set.Add( obj.GetMeshName() );
        }
        else
        {
          var obj = Context.GeometryGraph.objects.First( x => x.id == pair.Key );
          var meshName = obj.GetMeshName();
          set.Add( meshName );
        }
      }

      foreach(var obj in Context.GeometryGraph.objects)
      {
        var meshName = obj.GetMeshName();
        if ( meshName is null )
          continue;

        if ( meshName.Contains( "_LOD", StringComparison.InvariantCultureIgnoreCase ) )
          set.Add( meshName );
      }

      return set;
    }

    private HashSet<string> CreateVolumeMeshSet( Scene scene )
    {
      var set = new HashSet<string>();

      foreach ( var obj in Context.GeometryGraph.objects )
      {
        var objName = obj.GetName();

        if ( obj.Affixes is not null )
        {
          var affixes = obj.Affixes.Split( "\n", StringSplitOptions.RemoveEmptyEntries );
          foreach ( var affix in affixes )
          {
            if ( VolumeAffixes.Contains( affix ) )
            {
              set.Add( objName );
              break;
            }
            else if ( objName.Contains( "cdt", StringComparison.InvariantCultureIgnoreCase ) )
              set.Add( objName );
            else if ( objName.Contains( "_glr" ) )
              set.Add( objName );
            else if ( objName.StartsWith( "rb_" ) )
              set.Add( objName );

          }
        }
        else if ( objName.Contains( "cdt", StringComparison.InvariantCultureIgnoreCase ) )
          set.Add( objName );
        else if ( objName.Contains( "_glr" ) )
          set.Add( objName );
        else if ( objName.StartsWith( "rb_" ) )
          set.Add( objName );
      }

      //EvaluateMesh( set, scene, scene.RootNode, IsVolumeMesh );

      return set;
    }

    //private bool IsVolumeMesh( Scene scene, Node node )
    //{
    //  foreach ( var hint in _volumeStrings )
    //    if ( node.Name.Contains( hint ) )
    //      return true;

    //  return false;
    //}

    //private void EvaluateMesh( ISet<string> set, Scene scene, Node node, Func<Scene, Node, bool> evaluatorFunc )
    //{
    //  var name = node.Name;
    //  if ( evaluatorFunc( scene, node ) )
    //    set.Add( name );

    //  foreach ( var child in node.EnumerateChildren() )
    //    EvaluateMesh( set, scene, child, evaluatorFunc );
    //}

    #endregion

  }

}
