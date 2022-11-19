using Index.Domain.FileSystem;

namespace Index.Profiles.HaloCEA.FileSystem
{

  public class CEAFileNode : FileSystemNodeBase
  {

    #region Constants

    private const string META_START_OFFSET = "START_OFFSET";
    private const string META_FILE_SIZE = "FILE_SIZE";
    private const string META_FILE_TYPE = "FILE_TYPE";


    #endregion

    #region Properties

    public long StartOffset
    {
      get => GetMetadata<long>( META_START_OFFSET );
      set => SetMetadata<long>( META_START_OFFSET, value );
    }

    public long SizeInBytes
    {
      get => GetMetadata<long>( META_FILE_SIZE );
      set => SetMetadata<long>( META_FILE_SIZE, value );
    }

    public CEAFileType FileType
    {
      get => GetMetadata<CEAFileType>( META_FILE_TYPE );
      set => SetMetadata<CEAFileType>( META_FILE_TYPE, value );
    }

    #endregion

    #region Constructor

    public CEAFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion

  }

}
