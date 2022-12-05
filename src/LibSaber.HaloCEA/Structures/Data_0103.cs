using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0103 )]
  public struct Data_0103
  {

    #region Data Members

    public int count;
    public int Unk_01;

    #endregion

    #region Serialization

    public static Data_0103 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var obj = context.GetMostRecentObject<SaberObject>();
      var data = new Data_0103();

      var count = data.count = reader.ReadInt32();

      /* Not sure what this is doing... the disassembly needs some work to clarify.
       * The "geometry buffers" stuff is probably all wrong on my end.
       * 
       * unkReadBytes(unkReader, (int)&v19, 4, 4); - read 32bit int
       * if ( !sub_D1DD90(a1, v4, v19) )
       *   return 0;
       * v6 = a1->GeometryData;
       * if ( v6 && (v7 = v6->Buffers, (*(_QWORD *)&v7->flags & (256i64 << v4)) != 0) )
       *   unkReadBytes(unkReader, *(int *)((char *)&v7->flags + v22), 4 * v19, 2); - read 2x int16 per elem. compressed coords?
       * else
       *   unkReadBytes(unkReader, *(int *)((char *)&v6->Buffers->flags + v22), 8 * v19, 4); - read 2x floats per elem
       * 
       */

      return data;
    }

    #endregion

  }

}
