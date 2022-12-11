using Assimp;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class ConvertSceneLightsJob : JobBase
  {

    protected SaberScene Scene { get; set; }
    protected SceneContext Context { get; set; }

    public ConvertSceneLightsJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    protected override async Task OnInitializing()
    {
      Scene = Parameters.Get<SaberScene>();
      Context = Parameters.Get<SceneContext>();
    }

    protected override async Task OnExecuting()
    {
      SetStatus( "Loading Lights" );
      SetCompletedUnits( 0 );
      SetTotalUnits( Scene.Lights.Count );

      foreach ( var light in Scene.Lights )
      {
        AddLight( light );
        IncreaseCompletedUnits( 1 );
      }
    }

    private void AddLight( Data_0280_Entry sceneLight )
    {
      var lightName = sceneLight.Name;

      var light = new Light() { Name = lightName };
      var lightNode = new Node( lightName, Context.RootNode );
      Context.RootNode.Children.Add( lightNode );

      var matrix = sceneLight.Matrix.ToAssimp();
      matrix.Transpose();
      lightNode.Transform = matrix;

      var pos = sceneLight.Matrix;
      light.Position = new Vector3D( pos.M41, pos.M42, pos.M43 );

      switch ( sceneLight.LightInfo.LightType )
      {
        case 0:
          light.LightType = LightSourceType.Point;
          break;
        case 1:
          light.LightType = LightSourceType.Spot;
          break;
        case 4:
          light.LightType = LightSourceType.Directional;
          break;
      }

      light.ColorDiffuse = new Color3D(
        sceneLight.Color.X,
        sceneLight.Color.Y,
        sceneLight.Color.Z );
      light.ColorSpecular = light.ColorDiffuse;
      light.ColorAmbient = light.ColorDiffuse;

      light.AngleOuterCone = sceneLight.Data_0285.Unk_00;
      light.AngleInnerCone = sceneLight.Data_0285.Unk_01;
      light.Up = new Vector3D( 0, 0, -1 );

      Context.Scene.Lights.Add( light );
      IncreaseCompletedUnits( 1 );
    }

  }

}
