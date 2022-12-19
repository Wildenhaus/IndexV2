using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public class StringScriptingSerializer<T> : TextScriptingSerializer<T>
    where T : class, new()
  {

    #region Data Members

    private readonly Dictionary<Type, IScriptingSerializer> _serializerCache
      = new Dictionary<Type, IScriptingSerializer>();

    #endregion

    #region Public Methods

    public T Deserialize( NativeReader reader )
    {
      var obj = new T();

      OnDeserialize( reader, obj );

      return obj;
    }

    protected void OnDeserialize( NativeReader reader, T obj )
    {
      var stringData = reader.ReadLengthPrefixedString32();
      var stringReader = new StringReader( stringData );

      OnDeserialize( stringReader, obj );
    }

    #endregion

    #region Overrides

    protected override Dictionary<Type, IScriptingSerializer> GetSerializerCache()
      => _serializerCache;

    #endregion

  }

}
