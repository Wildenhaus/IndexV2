using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Serialization
{

    public abstract class CEASerializerBase<T> : SerializerBase<T>
  {

    #region Delegates

    protected delegate void ReadSentinelSectionDelegate( T data, NativeReader reader, Sentinel sentinel );

    #endregion

    #region Constructor

    protected CEASerializerBase( Stream stream )
      : base( stream )
    {
    }

    #endregion

    #region Private Methods

    protected Sentinel ReadSentinel()
      => Sentinel.Read( Reader );

    protected void ReadSentinelSection( T data, ReadSentinelSectionDelegate readFunc )
    {
      var sentinel = ReadSentinel();

      readFunc( data, Reader, sentinel );

      var readerPosition = Reader.Position;
      var sentinelEndOffset = sentinel.EndOffset;

      if ( readerPosition > sentinelEndOffset )
        FAIL( "Over-read Sentinel 0x{0:X2} block while deserializing type {1}.", sentinel.Id, typeof( T ).Name );
      else if ( readerPosition < sentinelEndOffset )
        FAIL( "Under-read Sentinel 0x{0:X2} block while deserializing type {1}.", sentinel.Id, typeof( T ).Name );
    }

    #endregion

  }

}
