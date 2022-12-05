using LibSaber.HaloCEA.Enumerations;

namespace LibSaber.HaloCEA.Files
{

  public class S3DPakFileEntry : CEAContainerFileEntryBase<S3DPakFile, S3DPakFileEntry>
  {

    #region Properties

    public CEAFileType Type { get; set; }

    #endregion

    #region Constructor

    public S3DPakFileEntry( S3DPakFile pakFile )
      : base( pakFile )
    {
    }

    #endregion

  }

}
