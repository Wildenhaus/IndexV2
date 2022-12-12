using Index.Domain.FileSystem;

namespace Index.Profiles.HaloCEA.FileSystem
{

  public class CEAFileSystemLoader : FileSystemLoaderBase
  {

    #region Constants

    // Exclude MP files - they just designate the base CE textures and scripting.
    // Everything else is just a stub.
    private static readonly string[] EXCLUDED_FILES = new string[]
    {
      "beavercreek",
      "bloodgulch",
      "boardingaction",
      "carousel",
      "chillout",
      "damnation",
      "dangercanyon",
      "deathisland",
      "gephyrophobia",
      "hangemhigh",
      "icefields",
      "infinity",
      "longest",
      "prisoner",
      "putput",
      "ratrace",
      "sidewinder",
      "timberland",
      "wizard"
    };

    #endregion

    #region Overrides

    protected override async Task OnLoadDevices()
    {
      await LoadFilesWithExtension( ".s3dpak", path => new S3DPakDevice( path ), EXCLUDED_FILES );
      await LoadFilesWithExtension( ".ipak", path => new IPakDevice( path ), EXCLUDED_FILES );
    }

    #endregion

  }

}
