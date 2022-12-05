using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_02E5 )]
  public struct Data_02E5
  {

    #region Constants

    private const int TEMPLATE_SIGNATURE = 0x4154504C; // ATPL

    #endregion

    #region Data Members

    public int Signature;

    public string Name;

    // This appears to always be null
    public string Unk_String;

    #endregion

    #region Serialization

    public static Data_02E5 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var info = new Data_02E5();

      var signature = info.Signature = reader.ReadInt32();
      ASSERT( signature == TEMPLATE_SIGNATURE, "Invalid Template signature." );

      info.Name = reader.ReadNullTerminatedString();

      // TODO: Figure out what this is
      // Seems to always be an empty string?
      info.Unk_String = reader.ReadNullTerminatedString();
      if ( !string.IsNullOrWhiteSpace( info.Unk_String ) )
        System.Diagnostics.Debugger.Break();

      return info;
    }

    #endregion

  }

}
