using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.IO;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class GeometryBufferSerializer : H2ASerializerBase<List<GeometryBuffer>>
  {

    #region Overrides

    public override List<GeometryBuffer> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var graph = context.GetMostRecentObject<GeometryGraph>();
      var sectionEndOffset = reader.ReadInt32();

      var count = graph.BufferCount;
      var buffers = new List<GeometryBuffer>( count );
      for ( var i = 0; i < count; i++ )
        buffers.Add( new GeometryBuffer() );

      var sentinelReader = new SentinelReader( reader );
      while ( reader.Position < sectionEndOffset )
      {
        sentinelReader.Next();
        switch ( sentinelReader.SentinelId )
        {
          case GeometryBufferSentinels.Flags:
            ReadBufferFlags( reader, buffers );
            break;
          case GeometryBufferSentinels.ElementSizes:
            ReadBufferElementSizes( reader, buffers );
            break;
          case GeometryBufferSentinels.Lengths:
            ReadBufferLengths( reader, buffers );
            break;
          case GeometryBufferSentinels.Data:
            ReadBufferData( reader, buffers );
            break;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      ASSERT( reader.BaseStream.Position == sectionEndOffset,
          "Reader position does not match the buffer section's end offset." );

      return buffers;
    }

    #endregion

    #region Private Methods

    private void ReadBufferFlags( NativeReader reader, List<GeometryBuffer> geometryBuffers )
    {
      foreach ( var buffer in geometryBuffers )
      {
        /* The data stored in each buffer is defined by a set of flags.
         * This is usually 0x3B or 0x3F in length.
         * It appears that it's always stored as a 64-bit int.
         */

        var flagCount = buffer.FlagSize = reader.ReadUInt16();
        var flagDataLength = Math.Ceiling( flagCount / 8f );
        ASSERT( flagDataLength == sizeof( ulong ), "GeometryBuffer flag data is not 8 bytes." );

        buffer.Flags = ( GeometryBufferFlags ) reader.ReadUInt64();
      }
    }

    private void ReadBufferElementSizes( NativeReader reader, List<GeometryBuffer> geometryBuffers )
    {
      /* The size of each element in the buffer (stride length).
       * We can use this to calculate offsets and catch cases where we under/overread each element.
       */

      foreach ( var buffer in geometryBuffers )
        buffer.ElementSize = reader.ReadUInt16();
    }

    private void ReadBufferLengths( NativeReader reader, List<GeometryBuffer> geometryBuffers )
    {
      /* The total length (in bytes) of the buffer.
       */

      foreach ( var buffer in geometryBuffers )
        buffer.BufferLength = reader.ReadUInt32();
    }

    private void ReadBufferData( NativeReader reader, List<GeometryBuffer> geometryBuffers )
    {
      /* This is the actual buffer data. We're using the previously obtained length
       * to determine the start and end offsets.
       * 
       * We can also optionally deserialize the buffer elements here.
       */

      foreach ( var buffer in geometryBuffers )
      {
        buffer.StartOffset = reader.BaseStream.Position;
        buffer.EndOffset = buffer.StartOffset + buffer.BufferLength;

        reader.BaseStream.Position = buffer.EndOffset;
      }
    }

    #endregion

    #region Sentinel Ids

    private class GeometryBufferSentinels
    {
      public const short Flags = 0x0000;
      public const short ElementSizes = 0x0001;
      public const short Lengths = 0x0002;
      public const short Data = 0x0003;
    }

    #endregion

  }

}