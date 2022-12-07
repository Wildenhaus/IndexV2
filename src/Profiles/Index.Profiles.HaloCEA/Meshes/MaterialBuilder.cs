using Assimp;

namespace Index.Profiles.HaloCEA.Meshes
{

  public class MaterialBuilder
  {

    #region Properties

    public SceneContext Context { get; }
    public string BaseTextureName { get; }

    #endregion

    #region Constructor

    private MaterialBuilder( SceneContext context, string baseTextureName )
    {
      Context = context;
      BaseTextureName = baseTextureName;
    }

    public static Material Build( SceneContext context, string baseTextureName )
    {
      var builder = new MaterialBuilder( context, baseTextureName );
      return builder.Build();
    }

    #endregion

    #region Private Methods

    private Material Build()
    {
      var material = new Material { Name = BaseTextureName };

      // Default Shader Parameters
      material.ColorSpecular = new Color4D( 0 );

      if ( TryFindTexture( BaseTextureName, out var diffusePath ) )
        material.TextureDiffuse = CreateTextureSlot( diffusePath, TextureType.Diffuse, 0, 0 );
      if ( TryFindTexture( BaseTextureName + "_nm", out var normalPath ) )
        material.TextureNormal = CreateTextureSlot( normalPath, TextureType.Normals, 0, 0 );
      if ( TryFindTexture( BaseTextureName + "_spec", out var specPath ) )
        material.TextureSpecular = CreateTextureSlot( specPath, TextureType.Specular, 0, 0 );

      return material;
    }

    private TextureSlot CreateTextureSlot(
      string filePath,
      TextureType typeSemantic,
      int texIndex,
      int uvIndex,
      TextureMapping mapping = TextureMapping.FromUV,
      float blendFactor = 0,
      TextureOperation texOp = TextureOperation.Add,
      TextureWrapMode wrapModeU = TextureWrapMode.Wrap,
      TextureWrapMode wrapModeV = TextureWrapMode.Wrap,
      int flags = 0 )
    {
      return new TextureSlot( filePath, typeSemantic, texIndex, mapping, uvIndex, blendFactor, texOp, wrapModeU, wrapModeV, flags );
    }

    private bool TryFindTexture( string textureName, out string path )
    {
      const string TEX_PATH = @"G:\h1a\textures_tga";
      path = default;

      var searchPattern = textureName + ".*";
      var files = Directory.GetFiles( TEX_PATH, searchPattern );
      if ( files.Length == 0 )
        return false;

      path = files[ 0 ];
      return true;
    }

    #endregion

  }

}
