namespace Index
{

  public interface IResult
  {

    #region Properties

    public bool IsSuccessful { get; }
    public string Message { get; }
    public Exception Exception { get; }

    #endregion

  }

  public interface IResult<out T> : IResult
  {

    #region Properties

    T Value { get; }

    #endregion

  }

  public class Result : IResult
  {

    #region Properties

    public bool IsSuccessful { get; }
    public string Message { get; }
    public Exception Exception { get; }

    #endregion

    #region Constructor

    public Result( bool isSuccessful, string message = null )
    {
      IsSuccessful = isSuccessful;
      Message = message;
    }

    public Result( Exception exception, string message = null )
    {
      IsSuccessful = false;
      Exception = exception;
      Message = message ?? exception?.Message;
    }

    public static Result Successful( string message = null )
      => new Result( true, message );

    public static Result<T> Successful<T>( T value, string message = null )
      => new Result<T>( true, value, message );

    public static Result Unsuccessful( string message = null )
      => new Result( false, message );

    public static Result Unsuccessful( Exception exception, string message = null )
      => new Result( exception, message );

    public static Result<T> Unsuccessful<T>( string message = null )
      => new Result<T>( false, message: message );

    public static Result Unsuccessful<T>( Exception exception, string message = null )
      => new Result<T>( exception, message );

    #endregion

  }

  public class Result<T> : Result, IResult<T>
  {

    #region Properties

    public T Value { get; }

    #endregion

    #region Constructor

    public Result( bool isSuccessful, T value = default, string message = null )
      : base( isSuccessful, message )
    {
      Value = value;
    }

    public Result( Exception exception, string message = null )
      : base( exception, message )
    {
    }

    #endregion

  }

}
