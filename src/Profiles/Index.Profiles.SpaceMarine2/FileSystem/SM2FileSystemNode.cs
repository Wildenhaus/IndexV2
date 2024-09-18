using System.IO.Compression;
using Index.Domain.FileSystem;

namespace Index.Profiles.SpaceMarine2.FileSystem;

public class SM2FileSystemNode : FileSystemNodeBase
{

  #region Properties

  internal ZipArchiveEntry Entry { get; set; }
  public long SizeInBytes { get; set; }

  #endregion

  #region Constructor

  public SM2FileSystemNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
  {
  }

  public SM2FileSystemNode( IFileSystemDevice device, ZipArchiveEntry entry, IFileSystemNode parent = null )
        : base( device, entry.FullName, parent )
  {
    Entry = entry;
    SizeInBytes = entry.Length;
  }

  #endregion

}
