namespace LibSaber.HaloCEA.Files
{

  public abstract class CEAContainerFileEntryBase<TFile, TEntry>
    where TFile : CEAContainerFileBase<TFile, TEntry>
    where TEntry : CEAContainerFileEntryBase<TFile, TEntry>
  {

    #region Data Members

    private readonly TFile _parentFile;

    #endregion

    #region Properties

    public string Name { get; set; }
    public int StartOffset { get; set; }
    public int SizeInBytes { get; set; }

    #endregion

    #region Constructor

    protected CEAContainerFileEntryBase( TFile parentFile )
    {
      ASSERT_NOT_NULL( parentFile );
      _parentFile = parentFile;
    }

    #endregion

    #region Public Methods

    public Stream GetStream()
      => _parentFile.GetStream( ( TEntry ) this );

    #endregion

    #region Overrides

    public override string ToString()
      => $"{_parentFile.Name}/{Name}";

    #endregion

  }

}
