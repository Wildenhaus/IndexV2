using LibSaber.FileSystem;

namespace LibSaber.Halo2A.Files
{

  public class SaberFileSystemNode : FileSystemNode
  {

    #region Properties

    public long StartOffset { get; }
    public long SizeInBytes { get; }

    #endregion

    #region Constructor

    public SaberFileSystemNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
      StartOffset = startOffset;
      SizeInBytes = sizeInBytes;
    }

    #endregion

  }

}
