namespace LibSaber.Serialization
{

  public interface IObjectList
  {
    Type ObjectType { get; }
  }

  public interface IObjectList<T> : IObjectList, IList<T>
  {
    Type IObjectList.ObjectType => typeof( T );
  }

}
