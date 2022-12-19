using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.FileSystem;

namespace Index.Profiles.Halo2A.FileSystem
{

  public class H2AFileSystemNode : FileSystemNodeBase
  {

    #region Properties

    public long StartOffset { get; }
    public long SizeInBytes { get; }

    #endregion

    #region Constructor

    public H2AFileSystemNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    public H2AFileSystemNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
      StartOffset = startOffset;
      SizeInBytes = sizeInBytes;
    }

    #endregion

  }

}
