using Index.Domain.FileSystem;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEACacheBlockEntryFileNode : FileSystemNodeBase
  {

    #region Properties

    public string EntryData { get; }

    #endregion

    #region Constructor

    public CEACacheBlockEntryFileNode( IFileSystemDevice device, string name, string entryData, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
      EntryData = entryData;
    }

    #endregion

  }

}
