using System.Collections;
using System.Reflection;
using System.Text;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public abstract class TextScriptingSerializer<T> : ScriptingSerializer<T>, ITextConfigurationSerializer<T>
    where T : class, new()
  {

    #region Constants

    private const int BUFFER_SIZE = 4096;

    #endregion

    #region Data Members

    private static readonly Dictionary<int, string> _arrayPropertyLookup;

    private int _propertiesRead;

    #endregion

    #region Constructor

    static TextScriptingSerializer()
    {
      _arrayPropertyLookup = BuildArrayPropertyLookup();
    }

    #endregion

    #region Overrides

    public T Deserialize( StringReader reader )
    {
      var obj = new T();

      OnDeserialize( reader, obj );

      return obj;
    }

    dynamic ITextScriptingSerializer.Deserialize( StringReader reader )
      => Deserialize( reader );

    protected override void OnDeserialize( Stream stream, T obj )
    {
      string stringData = null;
      using ( var streamReader = new StreamReader( stream, Encoding.UTF8, true, BUFFER_SIZE, true ) )
        stringData = streamReader.ReadToEnd();

      var reader = new StringReader( stringData );
      OnDeserialize( reader, obj );
    }

    protected void OnDeserialize( StringReader reader, T obj )
    {
      _propertiesRead = 0;
      while ( ReadProperty( reader, obj ) )
        continue;
    }

    #endregion

    #region Protected Methods

    protected bool ReadProperty( StringReader reader, T obj )
    {
      //if ( typeof( T ) == typeof( S3DColor ) )
      //  System.Diagnostics.Debugger.Break();

      var line = reader.ReadLine();

      // Check for EOF
      if ( line == null )
        return false;

      line = line.Trim();

      // Skip empty lines
      if ( string.IsNullOrWhiteSpace( line ) )
        return true;

      // Skip commented out lines
      if ( line.StartsWith( "//" ) )
        return true;

      if ( line.Contains( "}" ) )
        return false;

      if ( _arrayPropertyLookup.Count > 0 && line.Contains( "]" ) )
        return false;

      var (propertyName, propertyValue) = ParseTokens( line );

      dynamic parsedValue = ParseValue( reader, propertyName, propertyValue );
      SetPropertyValue( obj, propertyName, parsedValue );

      _propertiesRead++;
      return true;
    }

    #endregion

    #region Private Methods

    private (string name, string value) ParseTokens( string line )
    {
      var parts = line.Split( new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries );

      string propertyName;
      string propertyValue;
      if ( parts.Length < 2 && _arrayPropertyLookup.TryGetValue( _propertiesRead, out propertyName ) )
      {
        propertyValue = parts[ 0 ].Trim();
      }
      else if ( parts.Length > 2 )
      {
        // Handle quote-delimited property names with spaces
        parts = line.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
        var equalsIndex = Array.IndexOf( parts, "=" );
        var propertyNameParts = new ArraySegment<string>( parts, 0, equalsIndex );
        var propertyValueParts = new ArraySegment<string>( parts, equalsIndex + 1, parts.Length - equalsIndex - 1 );

        propertyName = String.Join( " ", propertyNameParts );
        propertyValue = String.Join( " ", propertyValueParts );

        propertyValue = propertyValue.Replace( "\"", "" ).Replace( ",", "" ).Trim();
        propertyName = propertyName.Replace( "\"", "" ).Trim();
      }
      else
      {
        propertyName = parts[ 0 ].Trim();
        propertyValue = parts[ 1 ].Replace( "\"", "" ).Replace( ",", "" ).Trim();
      }

      return (propertyName, propertyValue);
    }

    private dynamic ParseValue( StringReader reader, string propertyName, string propertyValue )
    {
      var property = GetProperty( propertyName );
      var propertyType = property.PropertyType;

      return ParseValue( reader, propertyType, propertyValue );
    }

    private dynamic ParseValue( StringReader reader, Type type, string value )
    {
      if ( type == typeof( int ) )
        return int.Parse( value );
      else if ( type == typeof( float ) )
        return float.Parse( value );
      else if ( type == typeof( bool ) )
        return bool.Parse( value );
      else if ( type == typeof( string ) )
        return value;
      else if ( type.IsArray )
      {
        if ( value.StartsWith( "[" ) )
          return ParseArray( reader, type );
        else
          return ParseSingleValueArray( reader, type, value );
      }
      else if ( typeof( IDictionary ).IsAssignableFrom( type ) )
      {
        if ( string.IsNullOrWhiteSpace( value ) )
          return null;
        else
        {
          if ( value.StartsWith( "{" ) )
            return ParseDictionary( reader, type );
          else
            return ParseSingleValueDictionary( reader, type, value );
        }
      }
      else
      {
        ASSERT( value.Contains( "{" ) || value.Contains( "[" ) );
        return ParseClass( reader, type );
      }
    }

    private dynamic ParseArray( StringReader reader, Type arrayType )
    {
      var elementType = arrayType.GetElementType();
      var elements = new List<dynamic>();

      while ( true )
      {
        var line = reader.ReadLine().Trim();
        if ( line == null || line.Contains( "]" ) )
          break;
        if ( string.IsNullOrWhiteSpace( line ) )
          continue;

        elements.Add( ParseValue( reader, elementType, line.Replace( ",", "" ) ) );
      }

      return elements.ToArray();
    }

    private dynamic ParseSingleValueArray( StringReader reader, Type arrayType, string value )
    {
      var elementType = arrayType.GetElementType();
      var elements = new List<dynamic>();

      elements.Add( ParseValue( reader, elementType, value ) );

      return elements.ToArray();
    }

    private dynamic ParseDictionary( StringReader reader, Type dictionaryType )
    {
      var keyType = dictionaryType.GenericTypeArguments[ 0 ];
      var valueType = dictionaryType.GenericTypeArguments[ 1 ];
      var dictionary = ( IDictionary ) Activator.CreateInstance( dictionaryType );

      while ( true )
      {
        var line = reader.ReadLine().Trim();
        if ( line == null || line.Contains( "}" ) )
          break;
        if ( string.IsNullOrWhiteSpace( line ) )
          continue;

        var (key, value) = ParseTokens( line );

        var parsedKey = ParseValue( reader, keyType, key );
        var parsedValue = ParseValue( reader, valueType, value );

        dictionary.Add( parsedKey, parsedValue );
      }

      return dictionary;
    }

    private dynamic ParseSingleValueDictionary( StringReader reader, Type dictionaryType, string value )
    {
      var keyType = dictionaryType.GenericTypeArguments[ 0 ];
      var valueType = dictionaryType.GenericTypeArguments[ 1 ];
      var dictionary = ( IDictionary ) Activator.CreateInstance( dictionaryType );

      dynamic defaultKey;
      if ( keyType == typeof( string ) )
        defaultKey = string.Empty;
      else
        defaultKey = Activator.CreateInstance( keyType );

      var parsedValue = ParseValue( reader, valueType, value );
      dictionary.Add( defaultKey, parsedValue );

      return dictionary;
    }

    private dynamic ParseClass( StringReader reader, Type classType )
    {
      var serializer = GetSerializerForType( classType ) as ITextScriptingSerializer;
      return serializer.Deserialize( reader );
    }

    private static Dictionary<int, string> BuildArrayPropertyLookup()
    {
      var lookup = new Dictionary<int, string>();

      var properties = typeof( T ).GetProperties();
      foreach ( var property in properties )
      {
        var attrs = property.GetCustomAttributes<ScriptingPropertyAttribute>();

        foreach ( var attr in attrs )
          if ( attr.ArrayIndex != -1 )
            lookup[ attr.ArrayIndex ] = attr.PropertyName;
      }

      return lookup;
    }

    #endregion

  }

}
