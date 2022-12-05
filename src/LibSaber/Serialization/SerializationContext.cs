namespace LibSaber.Serialization
{

  public class SerializationContext : ISerializationContext
  {

    #region Data Members

    private readonly Dictionary<Type, IObjectList> _objects;

    #endregion

    #region Constructor

    public SerializationContext()
    {
      _objects = new Dictionary<Type, IObjectList>();
    }

    #endregion

    #region Public Methods

    #region Object Tracking Methods

    public void AddObject<T>( T obj )
    {
      var objectList = GetOrCreateObjectList<T>();
      objectList.Add( obj );
    }

    public IObjectList<T> GetObjectList<T>()
      => GetOrCreateObjectList<T>();

    public T GetMostRecentObject<T>()
    {
      var objectList = GetOrCreateObjectList<T>();

      var objectCount = objectList.Count;
      if ( objectCount == 0 )
        FAIL( "No objects of type `{0}` were found.", objectList.ObjectType.Name );

      return objectList[ objectCount - 1 ];
    }

    #endregion

    #endregion

    #region Private Methods

    private IObjectList<T> GetOrCreateObjectList<T>()
    {
      var objectType = typeof( T );
      if ( !_objects.TryGetValue( objectType, out var objectList ) )
      {
        objectList = new ObjectList<T>();
        _objects.Add( objectType, objectList );
      }

      return ( IObjectList<T> ) objectList;
    }

    #endregion

  }

}
