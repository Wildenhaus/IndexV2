using System.Diagnostics;
using LibSaber.IO;
using LibSaber.Shared.Structures;

namespace LibSaber.Serialization
{

  public class SentinelReader<TOffset>
    where TOffset : unmanaged, IConvertible
  {

    #region Data Members

    private readonly Stream _stream;
    private readonly NativeReader _reader;

    private int _index;
    private Sentinel<TOffset> _currentSentinel;

    private bool _delimiterFlag;

    #endregion

    #region Properties

    public Sentinel<TOffset> Sentinel
    {
      get => _currentSentinel;
    }

    public short SentinelId
    {
      get => _currentSentinel.Id;
    }

    public long EndOffset
    {
      get => _currentSentinel.EndOffset;
    }

    #endregion

    #region Constructor

    public SentinelReader( NativeReader reader )
    {
      _stream = reader.BaseStream;
      _reader = reader;

      _index = -1;
      _currentSentinel = default;
    }

    #endregion

    #region Public Methods

    [DebuggerHidden]
    public bool Next( bool boundsCheck = true )
    {
      if ( boundsCheck && !_delimiterFlag && _index > -1 )
      {
        var difference = Math.Abs( EndOffset - _reader.Position );

        if ( EndOffset < _reader.Position )
          FAIL( "Over-read Sentinel 0x{0:X2} block by {1} byte(s).", Sentinel.Id, difference );
        else if ( EndOffset > _reader.Position )
          FAIL( "Under-read Sentinel 0x{0:X2} block by {1} byte(s).", Sentinel.Id, difference );
      }

      _index++;
      _currentSentinel = Sentinel<TOffset>.Read( _reader );
      _delimiterFlag = false;

      if ( _reader.Position == _stream.Length )
        return false;

      //Console.WriteLine( "0x{0:X2}", SentinelId );
      return true;
    }

    public void BurnSentinel()
    {
      Next();
      ASSERT( SentinelId == 1 );
    }

    public void SetDelimiterFlag()
      => _delimiterFlag = true;

    [DebuggerHidden]
    public void ReportUnknownSentinel()
      => FAIL( "Unknown Sentinel: 0x{0:X2}", SentinelId );

    #endregion

  }

}
