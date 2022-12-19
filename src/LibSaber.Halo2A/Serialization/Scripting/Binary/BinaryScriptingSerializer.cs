using System.Text;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public abstract class BinaryScriptingSerializer<T> : ScriptingSerializer<T>, IBinaryConfigurationSerializer<T>
    where T : class, new()
  {

    #region Public Methods

    public T Deserialize( NativeReader reader )
    {
      var obj = new T();

      OnDeserialize( reader, obj );

      return obj;
    }

    dynamic IBinaryScriptingSerializer.Deserialize( NativeReader reader )
      => Deserialize( reader );

    #endregion

    #region Abstract Methods

    protected abstract void ReadProperty( NativeReader reader, T obj );

    #endregion

    #region Overrides

    protected void OnDeserialize( NativeReader reader, T obj )
    {
      var propertyCount = reader.ReadUInt32();
      for ( var i = 0; i < propertyCount; i++ )
        ReadProperty( reader, obj );
    }

    protected override void OnDeserialize( Stream stream, T obj )
    {
      var reader = new NativeReader( stream, Endianness.LittleEndian );
      OnDeserialize( reader, obj );
    }

    #endregion

    #region Protected Methods

    protected DataType ReadDataType( NativeReader reader )
    {
      var dataType = ( DataType ) reader.ReadUInt32();

      if ( !Enum.IsDefined( typeof( DataType ), dataType ) )
        FAIL( "Unknown Configuration Property Data Type: {0:X}", dataType );

      return dataType;
    }

    protected dynamic ReadValue( NativeReader reader, DataType dataType, string propertyName )
    {
      dynamic readValue = null;

      switch ( dataType )
      {
        case DataType.Int:
          readValue = reader.ReadInt32();
          break;
        case DataType.Float:
          readValue = reader.ReadFloat32();
          break;
        case DataType.Bool:
          readValue = reader.ReadBoolean();
          break;
        case DataType.String:
          readValue = reader.ReadLengthPrefixedString32();
          break;
        case DataType.Array:
          readValue = ReadArray( reader, propertyName );
          break;
        case DataType.Class:
          readValue = ReadClass( reader, propertyName );
          break;
        default:
          FAIL( "Unhandled property: {0} {1}", dataType, propertyName );
          break;
      }

      return readValue;
    }

    protected dynamic ReadArray( NativeReader reader, string propertyName )
    {
      var count = reader.ReadUInt32();
      var array = new dynamic[ count ];

      for ( var i = 0; i < count; i++ )
      {
        var dataType = ReadDataType( reader );
        array[ i ] = ReadValue( reader, dataType, propertyName );
      }

      return array;
    }

    protected dynamic ReadClass( NativeReader reader, string propertyName )
    {
      var property = GetProperty( propertyName );
      var serializer = GetSerializerForType( property.PropertyType ) as IBinaryScriptingSerializer;

      return serializer.Deserialize( reader );
    }

    #endregion

    #region Embedded Types

    protected enum DataType : uint
    {
      Int = 1,
      Float = 2,
      Bool = 3,
      String = 4,
      Array = 6, //{ Int(DataType), Value }
      Class = 7
    }

    #endregion

  }

}