namespace LibSaber.Serialization
{

  public interface ISerializationContext
  {

    #region Public Methods

    void AddObject<T>( T obj );
    IObjectList<T> GetObjectList<T>();
    T GetMostRecentObject<T>();

    #endregion

  }

}
