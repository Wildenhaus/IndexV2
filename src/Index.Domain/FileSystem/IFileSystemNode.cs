namespace Index.Domain.FileSystem
{

  public interface IFileSystemNode : IDisposable
  {

    #region Properties

    IFileSystemDevice Device { get; }
    IParameterCollection Metadata { get; }

    string Name { get; }
    string DisplayName { get; }
    bool IsDirectory { get; }
    bool IsHidden { get; }

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

    T GetMetadata<T>( string key );
    void SetMetadata<T>( string key, T value );

    #endregion

  }

}
