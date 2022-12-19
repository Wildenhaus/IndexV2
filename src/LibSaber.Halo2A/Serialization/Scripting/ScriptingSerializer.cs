using System.Diagnostics;
using System.Reflection;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public abstract class ScriptingSerializer<T> : IConfigurationSerializer<T>
    where T : class, new()
  {

    #region Data Members

    private static readonly Dictionary<string, PropertyInfo> _propertyLookup;

    #endregion

    #region Constructor

    static ScriptingSerializer()
    {
      _propertyLookup = BuildPropertyLookup();
    }

    #endregion

    #region Public Methods

    public T Deserialize( Stream stream )
    {
      var obj = new T();

      OnDeserialize( stream, obj );

      return obj;
    }

    dynamic IScriptingSerializer.Deserialize( Stream stream )
      => Deserialize( stream );

    #endregion

    #region Abstract Methods

    protected abstract void OnDeserialize( Stream stream, T obj );

    protected abstract Dictionary<Type, IScriptingSerializer> GetSerializerCache();

    #endregion

    #region Protected Methods

    [DebuggerHidden]
    protected PropertyInfo GetProperty( string propertyName )
    {
      if ( !_propertyLookup.TryGetValue( propertyName, out var property ) )
        FAIL( "Unknown property for {0}: {1}",
          typeof( T ).Name,
          propertyName );

      return property;
    }

    protected IScriptingSerializer GetSerializerForType( Type type )
    {
      var serializerCache = GetSerializerCache();
      if ( !serializerCache.TryGetValue( type, out var serializer ) )
      {
        serializer = CreateSerializer( type );
        serializerCache.Add( type, serializer );
      }

      return serializer;
    }

    protected void SetPropertyValue( T obj, string propertyName, dynamic value )
    {
      if ( value is null )
        return;

      var property = GetProperty( propertyName );

      var objType = typeof( T );
      var propertyType = property.PropertyType;
      var valueType = ( Type ) value.GetType();

      if ( valueType.IsArray )
      {
        valueType = value[ 0 ].GetType().MakeArrayType();
        var newArray = Array.CreateInstance( value[ 0 ].GetType(), value.Length );
        Array.Copy( value, newArray, value.Length );
        value = newArray;
      }

      if ( property.DeclaringType != objType )
        FAIL( "Object-Property type mismatch for {0} {1}.{}: {}",
          propertyType.Name,
          typeof( T ).Name,
          propertyName,
          valueType.Name );

      if ( propertyType != valueType )
        FAIL( "Value-Property type mismatch for {0} {1}.{2}: {3}",
          propertyType.Name,
          typeof( T ).Name,
          propertyName,
          valueType.Name );

      property.SetValue( obj, value );
    }

    #endregion

    #region Private Methods

    private static Dictionary<string, PropertyInfo> BuildPropertyLookup()
    {
      var lookup = new Dictionary<string, PropertyInfo>();

      var properties = typeof( T ).GetProperties();
      foreach ( var property in properties )
      {
        var attrs = property.GetCustomAttributes<ScriptingPropertyAttribute>();
        foreach ( var attr in attrs )
          lookup.Add( attr.PropertyName, property );
      }

      return lookup;
    }

    private IScriptingSerializer CreateSerializer( Type type )
    {
      var genericSerializerType = GetType().GetGenericTypeDefinition();
      var definedSerializerType = genericSerializerType.MakeGenericType( type );

      return ( IScriptingSerializer ) Activator.CreateInstance( definedSerializerType );
    }

    #endregion

  }

}
