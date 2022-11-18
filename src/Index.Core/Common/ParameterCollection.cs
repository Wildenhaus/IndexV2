using System.Runtime.CompilerServices;

namespace Index
{

  public interface IParameterCollection : IDisposable
  {

    T Get<T>( string key );
    bool TryGet<T>( string key, out T parameter );

    void Set<T>( string key, T value );

  }

  public class ParameterCollection : DisposableObject, IParameterCollection
  {

    #region Data Members

    private Dictionary<string, object> _data;

    #endregion

    #region Public Methods

    public T Get<T>( string key )
    {
      if ( _data is null )
        return default;

      if ( !_data.TryGetValue( key, out var value ) )
        ThrowParameterNotFoundException( key );

      return ( T ) value;
    }

    public bool TryGet<T>( string key, out T parameter )
    {
      parameter = default;

      if ( _data is null )
        return false;

      if ( !_data.TryGetValue( key, out var value ) )
        return false;

      parameter = ( T ) value;
      return true;
    }

    public void Set<T>( string key, T value )
    {
      if ( _data is null )
        _data = new Dictionary<string, object>();

      _data.Add( key, value );
    }

    #endregion

    #region Private Methods

    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowParameterNotFoundException( string key )
      => throw new ParameterNotFoundException( key );

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      _data?.Clear();
    }

    #endregion

    #region Embedded Types

    public class ParameterNotFoundException : Exception
    {

      #region Properties

      public string Key { get; }

      #endregion

      #region Constructor

      public ParameterNotFoundException( string key )
        : base( $"Parameter not found: {key}" )
      {
        Key = key;
      }

      #endregion

    }

    #endregion

  }

}
