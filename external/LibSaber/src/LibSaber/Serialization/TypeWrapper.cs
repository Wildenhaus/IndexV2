namespace LibSaber.Serialization
{

  public class TypeWrapper<T>
  {

    #region Data Members

    public T Value;

    #endregion

    #region Operators

    public static implicit operator T( TypeWrapper<T> wrapper )
      => wrapper.Value;

    public static implicit operator TypeWrapper<T>( T value )
      => new TypeWrapper<T> { Value = value };

    #endregion

  }

}
