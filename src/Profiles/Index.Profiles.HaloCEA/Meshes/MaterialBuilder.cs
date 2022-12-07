using Assimp;
using Index.Domain.Assets.Textures;

namespace Index.Profiles.HaloCEA.Meshes
{

  public class MaterialBuilder
  {

    #region Properties

    public SceneContext Context { get; }
    public string BaseTextureName { get; }
    protected Dictionary<string, ITextureAsset> TextureLookup { get; }

    #endregion

    #region Constructor

    private MaterialBuilder( SceneContext context, string baseTextureName, Dictionary<string, ITextureAsset> textureLookup )
    {
      Context = context;
      BaseTextureName = baseTextureName;
      TextureLookup = textureLookup;
    }

    public static Material Build( SceneContext context, string baseTextureName, Dictionary<string, ITextureAsset> textureLookup )
    {
      var builder = new MaterialBuilder( context, baseTextureName, textureLookup );
      return builder.Build();
    }

    #endregion

    #region Private Methods

    private Material Build()
    {
      var material = new Material { Name = BaseTextureName };

      // Default Shader Parameters
      material.ColorSpecular = new Color4D( 0 );

      // Texture Slots
      if ( TextureExists( BaseTextureName ) )
        material.TextureDiffuse = CreateTextureSlot( BaseTextureName, TextureType.Diffuse, 0, 0 );

      var normalTextureName = BaseTextureName + "_nm";
      if ( TextureExists( normalTextureName ) )
        material.TextureNormal = CreateTextureSlot( normalTextureName, TextureType.Normals, 0, 0 );

      var specTextureName = BaseTextureName + "_spec";
      if ( TextureExists( specTextureName ) )
        material.TextureSpecular = CreateTextureSlot( specTextureName, TextureType.Specular, 0, 0 );

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

    private bool TextureExists( string textureName )
      => TextureLookup.ContainsKey( textureName );

    #endregion

  }

}
