using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public class DynamicBinaryScriptingSerializer<T> : BinaryScriptingSerializer<T>
    where T : class, new()
  {

    #region Data Members

    private readonly Dictionary<Type, IScriptingSerializer> _serializerCache
      = new Dictionary<Type, IScriptingSerializer>();

    #endregion

    #region Overrides

    protected override Dictionary<Type, IScriptingSerializer> GetSerializerCache()
      => _serializerCache;

    protected override void ReadProperty( NativeReader reader, T obj )
    {
      var propertyName = reader.ReadLengthPrefixedString32();
      var dataType = ReadDataType( reader );

      var propertyValue = ReadValue( reader, dataType, propertyName );
      SetPropertyValue( obj, propertyName, propertyValue );
    }

    #endregion

  }

}