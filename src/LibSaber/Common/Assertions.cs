using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LibSaber
{

  /// <summary>
  ///   A class that defines assertion macros.
  /// </summary>
  public static class Assertions
  {

    #region Assert Methods

    /// <summary>
    ///   Asserts that an expression is true.
    /// </summary>
    /// <param name="expression">
    ///   The expression to evaluate.
    /// </param>
    /// <param name="reason">
    ///   The reason the expression must be true.
    /// </param>
    [DebuggerHidden]
    public static void ASSERT( bool expression, [CallerArgumentExpression( "expression" )] string reason = null )
    {
      if ( !expression )
        ThrowAssertionFailedException( reason );
    }

    /// <summary>
    ///   Asserts that an expression is true.
    /// </summary>
    /// <param name="expression">
    ///   The expression to evaluate.
    /// </param>
    /// <param name="reasonFormat">
    ///   The format string for the message.
    /// </param>
    /// <param name="formatArgs">
    ///   The format arguments for the message.
    /// </param>
    [DebuggerHidden]
    public static void ASSERT( bool expression, string reasonFormat, params object[] formatArgs )
    {
      if ( !expression )
        ThrowAssertionFailedException( reasonFormat, formatArgs );
    }

    /// <summary>
    ///   Asserts that the provided value is not null.
    /// </summary>
    /// <param name="value">
    ///   The value that should not be null.
    /// </param>
    /// <param name="memberName">
    ///   The name of the member or variable.
    /// </param>
    [DebuggerHidden]
    public static void ASSERT_NOT_NULL<T>( T value, [CallerArgumentExpression( "value" )] string memberName = null )
      => ASSERT( value is not null, "Value is null: {0}.", memberName );

    #endregion

    #region Fail Methods

    /// <summary>
    ///   Immediately causes a failed assertion.
    /// </summary>
    [DebuggerHidden]
    public static void FAIL()
      => ThrowAssertionFailedException();

    /// <summary>
    ///   Immediately causes a failed assertion.
    /// </summary>
    /// <param name="reason">
    ///   The reason for the failure.
    /// </param>
    [DebuggerHidden]
    public static void FAIL( string reason )
      => ThrowAssertionFailedException( reason );

    /// <summary>
    ///   Immediately causes a failed assertion.
    /// </summary>
    /// <param name="reasonFormat">
    ///   The format string for the message.
    /// </param>
    /// <param name="formatArgs">
    ///   The format arguments for the message.
    /// </param>
    [DebuggerHidden]
    public static void FAIL( string reasonFormat, params object[] formatArgs )
      => ThrowAssertionFailedException( reasonFormat, formatArgs );

    /// <summary>
    ///   Immediately causes a failed assertion and returns the default
    ///   value of the specified generic data type.
    /// </summary>
    /// <typeparam name="T">
    ///   The generic data type to "return".
    /// </typeparam>
    /// <returns>
    ///   The default value of the generic data type.
    /// </returns>
    /// <remarks>
    ///   This is a convenience method for when you want to immediately punt
    ///   from a call that is designed to return a value.
    /// </remarks>
    [DebuggerHidden]
    public static T FAIL_RETURN<T>()
    {
      ThrowAssertionFailedException();
      return default;
    }

    /// <summary>
    ///   Immediately causes a failed assertion and returns the default
    ///   value of the specified generic data type.
    /// </summary>
    /// <typeparam name="T">
    ///   The generic data type to "return".
    /// </typeparam>
    /// <param name="reason">
    ///   The reason for the failure.
    /// </param>
    /// <returns>
    ///   The default value of the generic data type.
    /// </returns>
    /// <remarks>
    ///   This is a convenience method for when you want to immediately punt
    ///   from a call that is designed to return a value.
    /// </remarks>
    [DebuggerHidden]
    public static T FAIL_RETURN<T>( string reason )
    {
      ThrowAssertionFailedException( reason );
      return default;
    }

    /// <summary>
    ///   Immediately causes a failed assertion and returns the default
    ///   value of the specified generic data type.
    /// </summary>
    /// <typeparam name="T">
    ///   The generic data type to "return".
    /// </typeparam>
    /// <param name="reasonFormat">
    ///   The format string for the message.
    /// </param>
    /// <param name="formatArgs">
    ///   The format arguments for the message.
    /// </param>
    /// <returns>
    ///   The default value of the generic data type.
    /// </returns>
    /// <remarks>
    ///   This is a convenience method for when you want to immediately punt
    ///   from a call that is designed to return a value.
    /// </remarks>
    [DebuggerHidden]
    public static T FAIL_RETURN<T>( string reasonFormat, params object[] formatArgs )
    {
      ThrowAssertionFailedException( reasonFormat, formatArgs );
      return default;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    ///   Throws a generic <see cref="AssertionFailedException" />.
    /// </summary>
    [DebuggerHidden]
    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowAssertionFailedException()
      => throw new AssertionFailedException();

    /// <summary>
    ///   Throws an <see cref="AssertionFailedException" /> with a specified reason.
    /// </summary>
    /// <param name="reason">
    ///   The reason why the assertion failed.
    /// </param>
    [DebuggerHidden]
    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowAssertionFailedException( string reason )
      => throw new AssertionFailedException( reason );

    /// <summary>
    ///   Throws an <see cref="AssertionFailedException" /> with a specified reason.
    /// </summary>
    /// <param name="reasonFormat">
    ///   The format string for the message.
    /// </param>
    /// <param name="formatArgs">
    ///   The format arguments for the message.
    /// </param>
    [DebuggerHidden]
    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowAssertionFailedException( string reasonFormat, params object[] formatArgs )
      => throw new AssertionFailedException( reasonFormat, formatArgs );

    #endregion

    #region Embedded Types

    /// <summary>
    ///   An <see cref="Exception" /> that is thrown when an assertion fails.
    /// </summary>
    public class AssertionFailedException : Exception
    {

      #region Constants

      /// <summary>
      ///   A generic assertion failed message.
      /// </summary>
      private const string GENERIC_MESSAGE = "Assertion Failed.";

      #endregion

      #region Constructor

      /// <summary>
      ///   Constructs a new <see cref="AssertionFailedException" /> with a generic message.
      /// </summary>
      public AssertionFailedException()
        : base( GENERIC_MESSAGE )
      {
      }

      /// <summary>
      ///   Constructs a new <see cref="AssertionFailedException" /> with a specified message.
      /// </summary>
      /// <param name="message">
      ///   The assertion message.
      /// </param>
      public AssertionFailedException( string message )
        : base( BuildAssertionFailedMessage( message ) )
      {
      }

      /// <summary>
      ///   Constructs a new <see cref="AssertionFailedException" /> with a specified message.
      /// </summary>
      /// <param name="format">
      ///   The format of the assertion message.
      /// </param>
      /// <param name="formatArgs">
      ///   The arguments to use when formatting the assertion message.
      /// </param>
      public AssertionFailedException( string format, params object[] formatArgs )
        : base( BuildAssertionFailedMessage( format, formatArgs ) )
      {
      }

      #endregion

      #region Private Methods

      /// <summary>
      ///   Builds an assertion failed message.
      /// </summary>
      /// <param name="message">
      ///   The message.
      /// </param>
      /// <returns>
      ///   The formatted assertion failed message.
      /// </returns>
      private static string BuildAssertionFailedMessage( string message )
      {
        if ( string.IsNullOrWhiteSpace( message ) )
          return GENERIC_MESSAGE;

        return $"Assertion Failed: {message}";
      }

      /// <summary>
      ///   Builds an assertion failed message.
      /// </summary>
      /// <param name="format">
      ///   The message format string.
      /// </param>
      /// <param name="formatArgs">
      ///   The arguments for the message format string.
      /// </param>
      /// <returns>
      ///   The formatted assertion failed message.
      /// </returns>
      private static string BuildAssertionFailedMessage( string format, params object[] formatArgs )
      {
        if ( string.IsNullOrWhiteSpace( format ) )
          return GENERIC_MESSAGE;

        if ( formatArgs.Length == 0 )
          return format;

        return $"Assertion Failed: {string.Format( format, formatArgs )}";
      }

      #endregion

    }

    #endregion

  }

}