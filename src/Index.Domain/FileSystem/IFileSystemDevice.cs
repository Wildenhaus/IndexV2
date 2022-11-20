namespace Index.Domain.FileSystem
{

  public interface IFileSystemDevice : IDisposable
  {

    #region Properties

    bool IsInitialized { get; }
    bool IsDisposed { get; }

    IFileSystemNode Root { get; }
    IParameterCollection Metadata { get; }

    #endregion

    #region Public Methods

    IResult Initialize();
    Task<IResult> InitializeAsync( CancellationToken cancellationToken = default );
    Stream GetStream( IFileSystemNode node );

    IEnumerable<IFileSystemNode> EnumerateNodes();
    IEnumerable<IFileSystemNode> EnumerateDirectories();
    IEnumerable<IFileSystemNode> EnumerateFiles();

    #endregion

  }

}
