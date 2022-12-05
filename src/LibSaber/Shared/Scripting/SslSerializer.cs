using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace LibSaber.Shared.Scripting
{

  public static class SslSerializer
  {

    #region Public Methods

    public static SslObject Deserialize( Stream stream )
    {
      var reader = new SslReader( stream );
      return DeserializeObject( reader );
    }

    #endregion

    #region Private Methods

    private static SslObject DeserializeObject( SslReader reader )
    {
      dynamic sslObject = new SslObject();

      string identifier = null;
      var identifierStack = new Stack<string>();

      while ( reader.ReadNext() )
      {
        switch ( reader.TokenType )
        {
          case SslTokenType.Identifier:
            identifier = reader.Token;
            identifierStack.Push( identifier );
            break;

          case SslTokenType.Boolean:
          case SslTokenType.String:
          case SslTokenType.Integer:
          case SslTokenType.Double:
          {
            ASSERT_NOT_NULL( identifier );
            sslObject[ identifier ] = reader.TokenValue;
            identifier = null;
            break;
          }

          case SslTokenType.Assignment:
          {
            ASSERT_NOT_NULL( identifier );

            // Check for inline array
            if ( reader.RemainingTokens.Contains( "," )
              && !reader.RemainingTokens.Contains( "[" ) )
              sslObject[ identifier ] = DeserializeInlineArray( reader );

            break;
          }

          case SslTokenType.StartObject:
          {
            ASSERT_NOT_NULL( identifier );
            sslObject[ identifier ] = DeserializeObject( reader );
            identifier = null;
            break;
          }

          case SslTokenType.StartArray:
          {
            ASSERT_NOT_NULL( identifier );
            sslObject[ identifier ] = DeserializeArray( reader );
            identifier = null;
            break;
          }

          case SslTokenType.Accessor:
          {
            ASSERT_NOT_NULL( identifier );
            var parent = sslObject[ identifier ] as SslObject;
            if ( parent is null )
              parent = sslObject[ identifier ] = new SslObject();

            var child = DeserializeObject( reader );
            parent.Merge( child );

            break;
          }

          case SslTokenType.EndObject:
            return sslObject;

          default:
            ThrowUnexpectedTokenException( reader );
            break;
        }
      }

      return sslObject;
    }

    private static SslArray DeserializeArray( SslReader reader )
    {
      dynamic array = new SslArray();

      int count = 0;
      while ( reader.ReadNext() )
      {
        switch ( reader.TokenType )
        {
          case SslTokenType.Boolean:
          case SslTokenType.String:
          case SslTokenType.Integer:
          case SslTokenType.Double:
            array.Push( reader.TokenValue );
            break;

          case SslTokenType.StartObject:
            array.Push( DeserializeObject( reader ) );
            break;

          case SslTokenType.EndArray:
            return array;

          case SslTokenType.Separator:
            break;

          case SslTokenType.Identifier:
          {
            // TODO: This probably means it's a constant.
            // These have actual values, but we're just treating them as a string.
            array.Push( reader.Token );
            break;
          }

          default:
            ThrowUnexpectedTokenException( reader );
            break;
        }
      }

      ThrowIncompleteScopeException();
      return null;
    }

    private static SslArray DeserializeInlineArray( SslReader reader )
    {
      var array = new SslArray();

      var remainingTokens = reader.RemainingTokens.ToArray();
      foreach ( var token in remainingTokens )
      {
        reader.ReadNext();
        switch ( reader.TokenType )
        {
          case SslTokenType.Boolean:
          case SslTokenType.String:
          case SslTokenType.Integer:
          case SslTokenType.Double:
            array.Push( reader.TokenValue );
            break;

          case SslTokenType.Identifier:
          {
            // TODO: This probably means it's a constant.
            // These have actual values, but we're just treating them as a string.
            array.Push( reader.Token );
            break;
          }

          case SslTokenType.Separator:
            break;

          default:
            ThrowUnexpectedTokenException( reader );
            break;
        }
      }

      return array;
    }

    [DebuggerHidden]
    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowUnexpectedTokenException( SslReader reader )
      => throw new InvalidDataException( $"Unexpected token: {reader.Token} ({reader.TokenType})" );

    [DebuggerHidden]
    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowIncompleteScopeException()
      => throw new InvalidDataException( "Reached the end of the script while still inside of a scope." );

    #endregion

  }

}
