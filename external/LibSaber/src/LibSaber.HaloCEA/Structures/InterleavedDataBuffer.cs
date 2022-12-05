using LibSaber.HaloCEA.Enumerations;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectInterleavedBuffer )]
  public class InterleavedDataBuffer
  {

    #region Data Members

    public int Count;
    public BitSet<short> Flags;

    public byte ElementSize;
    public InterleavedData[] ElementData;

    #endregion

    #region Serialization

    public static InterleavedDataBuffer Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new InterleavedDataBuffer();
      context.AddObject( data );

      data.Count = reader.ReadInt32();
      data.Flags = BitSet<short>.Deserialize( reader, context );
      data.ElementSize = reader.ReadByte();

      data.ElementData = new InterleavedData[ data.Count ];
      for ( var i = 0; i < data.Count; i++ )
      {
#if DEBUG
        var startPos = reader.Position;
#endif
        data.ElementData[ i ] = InterleavedData.Deserialize( reader, context );
#if DEBUG
        var flags = data.Flags.GetFlags<InterleavedDataFlags>();
        var elementSize = data.ElementSize;
        var bytesRead = reader.Position - startPos;
        if ( bytesRead > data.ElementSize )
          FAIL( "Over-read Interleaved Datum by {0} bytes.", bytesRead - elementSize );
        else if ( bytesRead < data.ElementSize )
          FAIL( "Under-read Interleaved Datum by {0} bytes.", elementSize - bytesRead );
#endif
      }

      return data;
    }

    #endregion

  }

}
