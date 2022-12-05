namespace LibSaber.IO
{

  public class StreamSegment : Stream
  {

    #region Data Members

    private readonly Stream _baseStream;

    private readonly long _startOffset;
    private readonly long _endOffset;
    private readonly long _length;

    private long _position;

    #endregion

    #region Properties

    public long StartOffset
    {
      get
      {
        if ( _baseStream is StreamSegment segment )
          return segment.StartOffset + _startOffset;

        return _startOffset;
      }
    }

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => _baseStream.CanSeek;
    public override bool CanWrite => _baseStream.CanWrite;

    public override long Length => _length;
    public override long Position
    {
      get => _position;
      set
      {
        var newPosition = value;
        ASSERT( newPosition >= 0, "Position cannot be negative." );
        ASSERT( newPosition < _length, "Position is out of bounds." );

        _position = newPosition;
        _baseStream.Position = _startOffset + newPosition;
      }
    }

    #endregion

    #region Constructor

    public StreamSegment( Stream baseStream, long startOffset, long length )
    {
      _baseStream = baseStream;

      _startOffset = startOffset;
      _endOffset = _startOffset + length;
      _length = length;

      _position = 0;
      _baseStream.Position = startOffset;
    }

    #endregion

    #region Overrides

    public override int Read( byte[] buffer, int offset, int count )
    {
      var expectedPos = _startOffset + _position;

      // Ensure we're at the proper position
      if ( _baseStream.Position != expectedPos )
        Seek( _position, SeekOrigin.Begin );

      // Prevent out-of-bounds
      long bytesToRead = offset + count;
      if ( _position + bytesToRead > _endOffset )
        bytesToRead = _endOffset - _position - offset;

      if ( bytesToRead > _length - _position )
        bytesToRead = Math.Max( 0, _length - _position );

      if ( bytesToRead <= 0 )
        return 0;

      var bytesRead = _baseStream.Read( buffer, offset, ( int ) bytesToRead );
      _position += bytesRead;
      return bytesRead;
    }

    public override long Seek( long offset, SeekOrigin origin )
    {
      switch ( origin )
      {
        case SeekOrigin.Begin:
          offset = offset + _startOffset;
          break;
        case SeekOrigin.End:
          offset = offset + _endOffset;
          break;
        case SeekOrigin.Current:
          offset = offset + _position;
          break;
      }

      offset = Math.Max( _startOffset, offset );
      offset = Math.Min( _endOffset, offset );

      _baseStream.Seek( offset, SeekOrigin.Begin );
      _position = offset - _startOffset;

      return _position;
    }

    public override void Flush()
      => _baseStream.Flush();

    public override void SetLength( long value )
      => _baseStream.SetLength( value );

    public override void Write( byte[] buffer, int offset, int count )
      => _baseStream.Write( buffer, offset, count );

    #endregion

  }

}
