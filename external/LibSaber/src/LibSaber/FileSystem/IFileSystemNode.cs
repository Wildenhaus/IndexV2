namespace LibSaber.FileSystem
{

  public interface IFileSystemNode : IDisposable
  {

    #region Properties

    IFileSystemDevice Device { get; }

    string Name { get; }
    string DisplayName { get; }
    bool IsDirectory { get; }

    IFileSystemNode Parent { get; }
    IFileSystemNode FirstChild { get; set; }
    IFileSystemNode NextSibling { get; set; }

    #endregion

    #region Public Methods

    void AddChild( IFileSystemNode node );
    void AddSibling( IFileSystemNode node );

    IEnumerable<IFileSystemNode> EnumerateChildren( bool recursive = false );
    IEnumerable<IFileSystemNode> EnumerateSiblings( bool includeThis = false );

    string GetPath( bool excludeRoot = true );
    Stream Open();

    #endregion

  }

}
