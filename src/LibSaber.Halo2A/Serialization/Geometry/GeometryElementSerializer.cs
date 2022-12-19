using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Geometry
{

  public abstract class GeometryElementSerializer<T>
  {

    #region Data Members

    private int _currentIndex;

    #endregion

    #region Properties

    protected NativeReader Reader { get; }
    protected GeometryBuffer Buffer { get; }

    protected GeometryBufferFlags Flags => Buffer.Flags;
    protected ushort ElementSize => Buffer.ElementSize;
    protected int ElementCount => Buffer.Count;

    public T Current { get; private set; }

    #endregion

    #region Constructor

    protected GeometryElementSerializer( NativeReader reader, GeometryBuffer buffer )
    {
      Reader = reader;
      Buffer = buffer;

      EnsureReaderIsAtBufferStart( reader, buffer );
    }

    #endregion

    #region Public Methods

    public bool MoveNext()
    {
      if ( _currentIndex == ElementCount )
        return false;

      var lastPosition = Reader.Position;
      Current = ReadElement();

      var bytesRead = Reader.Position - lastPosition;
      if ( bytesRead > ElementSize )
        FAIL( "Element over-read. Expected: {0}, Actual: {1}", ElementSize, bytesRead );
      else if ( bytesRead < ElementSize )
        FAIL( "Element under-read. Expected: {0}, Actual: {1}", ElementSize, bytesRead );

      _currentIndex++;
      return true;
    }

    #endregion

    #region Private Methods

    private static void EnsureReaderIsAtBufferStart( NativeReader reader, GeometryBuffer buffer )
    {
      if ( reader.Position != buffer.StartOffset )
        reader.Position = buffer.StartOffset;
    }

    protected abstract T ReadElement();

    #endregion

  }

}
