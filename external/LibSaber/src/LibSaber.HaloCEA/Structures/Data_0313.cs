using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0313 )]
  public struct Data_0313
  {

    #region Data Members

    public string Unk_00;
    public int Unk_01;

    public byte[] HavokData;

    #endregion

    #region Serialization

    public static Data_0313 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0313();

      data.Unk_00 = reader.ReadLengthPrefixedString32();

      var count = data.Unk_01 = reader.ReadInt32();
      var havokData = data.HavokData = new byte[ count ];
      reader.Read( havokData );

      return data;
    }

    #endregion

  }

}
