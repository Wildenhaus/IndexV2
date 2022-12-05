using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0316 )]
  public class Data_0316
  {

    #region Data Members

    public byte Unk_00;
    public byte Unk_01;
    public byte Unk_02;

    #endregion

    #region Serialization

    public static Data_0316 Deserialize( NativeReader reader, ISerializationContext context )
    {
      /* Unknown data.
       * 
       * This appears to be a length-prefixed bitfield.
       * The length of the bitfield is always a bit.
       */

      var data = new Data_0316();

      var unk_count = data.Unk_00 = reader.ReadByte();
      if ( unk_count > 0 )
        data.Unk_01 = reader.ReadByte();
      if ( unk_count > 1 )
        data.Unk_02 = reader.ReadByte();

      if ( unk_count > 2 )
        FAIL( "Unexpected count on Sentinel 0316" );

      return data;
    }

    #endregion

  }

}
