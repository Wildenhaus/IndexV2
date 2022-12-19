using System.Collections;
using System.Numerics;
using System.Text;

namespace LibSaber.IO
{

  public sealed class NativeReader
  {

    #region Properties

    public Stream BaseStream { get; }

    public Endianness Endianness { get; }

    public long Length
    {
      get => BaseStream.Length;
    }

    public long Position
    {
      get => BaseStream.Position;
      set => BaseStream.Position = value;
    }

    private bool NeedsByteOrderSwap { get; }

    #endregion

    #region Constructor

    public NativeReader( Stream baseStream, Endianness endianness )
    {
      BaseStream = baseStream;
      Endianness = endianness;
      NeedsByteOrderSwap = BitConverter.IsLittleEndian ^ ( Endianness == Endianness.LittleEndian );
    }

    #endregion

    #region Public Methods

    public int Read( Span<byte> buffer ) => BaseStream.Read( buffer );

    public Boolean ReadBoolean() => ReadUnmanaged<Boolean>();
    public Byte ReadByte() => ReadUnmanaged<Byte>();
    public SByte ReadSByte() => ReadUnmanaged<SByte>();

    public Int16 ReadInt16() => ReadUnmanaged<Int16>();
    public UInt16 ReadUInt16() => ReadUnmanaged<UInt16>();
    public Int32 ReadInt32() => ReadUnmanaged<Int32>();
    public UInt32 ReadUInt32() => ReadUnmanaged<UInt32>();
    public Int64 ReadInt64() => ReadUnmanaged<Int64>();
    public UInt64 ReadUInt64() => ReadUnmanaged<UInt64>();

    public Half ReadFloat16() => ReadUnmanaged<Half>();
    public Single ReadFloat32() => ReadUnmanaged<Single>();
    public Double ReadFloat64() => ReadUnmanaged<Double>();
    public Decimal ReadFloat128() => ReadUnmanaged<Decimal>();

    public Guid ReadGuid() => ReadUnmanaged<Guid>();

    public string ReadFixedLengthString( int length )
    {
      var pos = 0;
      var sb = new StringBuilder( length );

      while ( length - pos >= 8 )
      {
        var data = ReadUnmanaged<Int64>();
        sb.Append( ( char ) ( ( data >> 0x00 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x08 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x10 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x18 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x20 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x28 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x30 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x38 ) & 0xFF ) );
        pos += 8;
      }

      if ( length - pos >= 4 )
      {
        var data = ReadUnmanaged<Int32>();
        sb.Append( ( char ) ( ( data >> 0x00 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x08 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x10 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x18 ) & 0xFF ) );
        pos += 4;
      }

      if ( length - pos >= 2 )
      {
        var data = ReadUnmanaged<Int16>();
        sb.Append( ( char ) ( ( data >> 0x00 ) & 0xFF ) );
        sb.Append( ( char ) ( ( data >> 0x08 ) & 0xFF ) );
        pos += 2;
      }

      if ( length - pos == 1 )
      {
        var data = ReadUnmanaged<Byte>();
        sb.Append( ( char ) data );
        pos += 1;
      }

      ASSERT( pos == length, "Incorrectly read fixed-length string." );
      return sb.ToString();
    }

    public string ReadNullTerminatedString()
    {
      // TODO: Is there a faster way of doing this?
      var sb = new StringBuilder();

      var c = ReadUnmanaged<Byte>();
      while ( c != 0 )
      {
        sb.Append( ( char ) c );
        c = ReadUnmanaged<Byte>();
      }

      return sb.ToString();
    }

    public string ReadLengthPrefixedString16()
      => ReadFixedLengthString( ReadUnmanaged<Int16>() );

    public string ReadLengthPrefixedString32()
      => ReadFixedLengthString( ReadUnmanaged<Int32>() );

    public Vector3 ReadVector3()
      => new Vector3( ReadFloat32(), ReadFloat32(), ReadFloat32() );

    public Vector4 ReadVector4()
      => new Vector4( ReadFloat32(), ReadFloat32(), ReadFloat32(), ReadFloat32() );

    public Matrix4x4 ReadMatrix4x4()
    {
      return new Matrix4x4(
        ReadFloat32(), ReadFloat32(), ReadFloat32(), ReadFloat32(),
        ReadFloat32(), ReadFloat32(), ReadFloat32(), ReadFloat32(),
        ReadFloat32(), ReadFloat32(), ReadFloat32(), ReadFloat32(),
        ReadFloat32(), ReadFloat32(), ReadFloat32(), ReadFloat32()
        );
    }

    public BitArray ReadBitArray( int bitCount )
    {
      var readLen = ( int ) Math.Ceiling( bitCount / 8f );
      var buffer = new byte[ readLen ];
      Read( buffer );

      return new BitArray( buffer );
    }

    public byte PeekByte()
    {
      var b = ReadByte();
      Position -= 1;
      return b;
    }

    public void Seek( long offset, SeekOrigin origin = SeekOrigin.Begin )
      => BaseStream.Seek( offset, origin );

    #endregion

    #region Private Methods

    internal unsafe T ReadUnmanaged<T>()
      where T : unmanaged
    {
      var alloc = stackalloc byte[ sizeof( T ) ];
      var buffer = new Span<byte>( alloc, sizeof( T ) );
      BaseStream.Read( buffer );

      if ( NeedsByteOrderSwap )
        buffer.Reverse();

      return *( T* ) alloc;
    }

    #endregion

  }

}
