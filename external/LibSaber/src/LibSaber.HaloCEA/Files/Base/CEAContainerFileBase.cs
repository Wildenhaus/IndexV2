using LibSaber.HaloCEA.IO;
using LibSaber.IO;

namespace LibSaber.HaloCEA.Files
{

  public abstract class CEAContainerFileBase<TFile, TEntry>
    where TFile : CEAContainerFileBase<TFile, TEntry>
    where TEntry : CEAContainerFileEntryBase<TFile, TEntry>
  {

    #region Data Members

    private readonly string _filePath;
    private CEAStreamCompressionInfo _compressionInfo;

    private Dictionary<string, TEntry> _entries;

    #endregion

    #region Properties

    public string Name { get; }
    public string FilePath => _filePath;
    public CEAStreamCompressionInfo CompressionInfo => _compressionInfo;

    public IReadOnlyDictionary<string, TEntry> Entries => _entries;

    #endregion

    #region Constructor

    protected CEAContainerFileBase( string filePath, CEAStreamCompressionInfo compressionInfo = default )
    {
      ASSERT( File.Exists( filePath ), "File not found: {0}", filePath );

      _filePath = filePath;
      _compressionInfo = compressionInfo;
      _entries = new Dictionary<string, TEntry>();

      Name = Path.GetFileNameWithoutExtension( _filePath );
    }

    #endregion

    #region Public Methods

    public virtual Stream GetStream()
    {
      var stream = CEAStream.FromFile( _filePath, _compressionInfo );
      _compressionInfo = stream.CompressionInfo;

      return stream;
    }

    public virtual Stream GetStream( TEntry entry )
    {
      ASSERT_NOT_NULL( entry );

      var stream = GetStream();
      return new StreamSegment( stream, entry.StartOffset, entry.SizeInBytes );
    }

    #endregion

    #region Private Methods

    protected void AddEntry( TEntry entry )
    {
      ASSERT_NOT_NULL( entry );
      _entries.Add( entry.Name, entry );
    }

    #endregion

  }

}
