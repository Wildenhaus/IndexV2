using LibSaber.IO;

namespace LibSaber.Serialization
{

  public abstract class SerializerBase<T>
  {

    #region Data Members

    private readonly Stream _stream;
    private readonly NativeReader _reader;

    #endregion

    #region Properties

    protected Stream Stream
    {
      get => _stream;
    }

    protected NativeReader Reader
    {
      get => _reader;
    }

    #endregion

    #region Constructor

    protected SerializerBase( Stream stream )
    {
      _stream = stream;
      _reader = CreateReader( stream );
    }

    #endregion

    #region Public Methods

    public abstract T Deserialize();

    #endregion

    #region Private Methods

    protected virtual NativeReader CreateReader( Stream stream )
    {
      return new NativeReader( stream, Endianness.LittleEndian );
    }

    #endregion

  }

}
