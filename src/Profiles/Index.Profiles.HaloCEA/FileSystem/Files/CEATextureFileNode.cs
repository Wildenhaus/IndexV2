using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.FileSystem;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEATextureFileNode : CEAFileNode
  {

    #region Constants

    private const string META_TEX_WIDTH = "TEXTURE_WIDTH";
    private const string META_TEX_HEIGHT = "TEXTURE_HEIGHT";
    private const string META_TEX_DEPTH = "TEXTURE_DEPTH";
    private const string META_TEX_MIPCOUNT = "TEXTURE_MIP_CNT";
    private const string META_TEX_FACECOUNT = "TEXTURE_FACE_COUNT";
    private const string META_TEX_FORMAT = "TEXTURE_FORMAT";

    #endregion

    #region Properties

    public int Width
    {
      get => GetMetadata<int>( META_TEX_WIDTH );
      set => SetMetadata<int>( META_TEX_WIDTH, value );
    }

    public int Height
    {
      get => GetMetadata<int>( META_TEX_HEIGHT );
      set => SetMetadata<int>( META_TEX_HEIGHT, value );
    }

    public int Depth
    {
      get => GetMetadata<int>( META_TEX_DEPTH );
      set => SetMetadata<int>( META_TEX_DEPTH, value );
    }

    public int MipCount
    {
      get => GetMetadata<int>( META_TEX_MIPCOUNT );
      set => SetMetadata<int>( META_TEX_MIPCOUNT, value );
    }

    public int FaceCount
    {
      get => GetMetadata<int>( META_TEX_FACECOUNT );
      set => SetMetadata<int>( META_TEX_FACECOUNT, value );
    }

    public CEATextureFormat Format
    {
      get => GetMetadata<CEATextureFormat>( META_TEX_FORMAT );
      set => SetMetadata<CEATextureFormat>( META_TEX_FORMAT, value );
    }

    #endregion

    #region Constructor

    public CEATextureFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion


  }

}
