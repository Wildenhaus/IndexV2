using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace LibSaber.Shared.Scripting
{

  public class SslReader
  {

    #region Constants

    private const string PATTERN_WHITESPACE = @"^[\s\t]+";
    private const string PATTERN_SEPARATOR = @"^\,";
    private const string PATTERN_ACCESSOR = @"^\.";
    private const string PATTERN_IDENTIFIER = @"^[a-zA-Z_]\w*";
    private const string PATTERN_ASSIGNMENT = @"^\=";
    private const string PATTERN_OPEN_PARENTHESIS = @"^\(";
    private const string PATTERN_CLOSE_PARENTHESIS = @"^\)";
    private const string PATTERN_OPEN_BRACE = @"^\{";
    private const string PATTERN_CLOSE_BRACE = @"^\}";
    private const string PATTERN_OPEN_BRACKET = @"^\[";
    private const string PATTERN_CLOSE_BRACKET = @"^\]";
    private const string PATTERN_OPEN_SSL_SCRIPT = @"^#ssl_begin";
    private const string PATTERN_CLOSE_SSL_SCRIPT = @"^#ssl_end";
    private const string PATTERN_OPEN_SSL_SCRIPT_INLINE = @"^<<<";
    private const string PATTERN_CLOSE_SSL_SCRIPT_INLINE = @"^>>>";
    private const string PATTERN_STRING = @"^""(?:[^""\\]|\\.)*""";
    private const string PATTERN_FLOAT = @"^-?\d+\.\d+";
    private const string PATTERN_INT = @"^-?\d+";
    private const string PATTERN_COMMENT = @"^\/\/.*";

    private static readonly Regex[] PATTERNS = new Regex[]
    {
      new Regex( PATTERN_WHITESPACE ),
      new Regex( PATTERN_SEPARATOR ),
      new Regex( PATTERN_ACCESSOR ),
      new Regex( PATTERN_IDENTIFIER ),
      new Regex( PATTERN_ASSIGNMENT ),
      new Regex( PATTERN_OPEN_PARENTHESIS ),
      new Regex( PATTERN_CLOSE_PARENTHESIS ),
      new Regex( PATTERN_OPEN_BRACE ),
      new Regex( PATTERN_CLOSE_BRACE ),
      new Regex( PATTERN_OPEN_BRACKET ),
      new Regex( PATTERN_CLOSE_BRACKET ),
      new Regex( PATTERN_OPEN_SSL_SCRIPT ),
      new Regex( PATTERN_CLOSE_SSL_SCRIPT ),
      new Regex( PATTERN_OPEN_SSL_SCRIPT_INLINE ),
      new Regex( PATTERN_CLOSE_SSL_SCRIPT_INLINE ),
      new Regex( PATTERN_STRING ),
      new Regex( PATTERN_FLOAT ),
      new Regex( PATTERN_INT ),
      new Regex( PATTERN_COMMENT ),
    };

    #endregion

    #region Data Members

    private StreamReader _reader;
    private Queue<string> _tokens;
    private Stack<SslTokenType> _scopeStack;
    private dynamic _tokenValue;

    #endregion

    #region Properties

    public string Token { get; private set; }
    public SslTokenType TokenType { get; private set; }

    public IEnumerable<string> RemainingTokens
    {
      get => _tokens;
    }

    public dynamic TokenValue
    {
      get => _tokenValue;
      private set => _tokenValue = value;
    }

    #endregion

    #region Constructor

    public SslReader( Stream stream )
    {
      _reader = new StreamReader( stream );
      _tokens = new Queue<string>();
      _scopeStack = new Stack<SslTokenType>();
    }

    #endregion

    #region Public Methods

    public bool ReadNext()
    {
      if ( _tokens.Count == 0 )
      {
        if ( !TokenizeNextLine() )
          return false;
      }

      Token = _tokens.Dequeue();
      TokenType = DetermineTokenType( Token );

      return true;
    }

    #endregion

    #region Private Methods

    private bool TokenizeNextLine()
    {
      while ( !_reader.EndOfStream )
      {
        var line = _reader.ReadLine().AsSpan();
        for ( var i = 0; i < line.Length; )
        {
          var token = ReadToken( line.Slice( i ), ref i );
          if ( token.IsEmpty )
            ThrowTokenParseFailedException();

          if ( token.SequenceEqual( "<<<" ) || token.SequenceEqual( "#ssl_begin" ) )
            token = ReadSslScripting();

          if ( token.IsWhiteSpace() )
            continue;

          _tokens.Enqueue( token.Trim().ToString() );
        }

        // No tokens on current line. Try again.
        if ( _tokens.Count == 0 )
          continue;

        return true;
      }

      return false;
    }

    [MethodImpl( MethodImplOptions.AggressiveOptimization )]
    private static ReadOnlySpan<char> ReadToken( ReadOnlySpan<char> line, ref int index )
    {
      // .NET Regex doesn't currently support spans.
      // Gonna have to eat the overhead until there's a better way.
      var lineString = line.ToString();

      foreach ( var pattern in PATTERNS )
      {
        var result = pattern.Match( lineString );
        if ( !result.Success )
          continue;

        var token = result.ValueSpan;
        index += token.Length;
        return token;
      }

      return ReadOnlySpan<char>.Empty;
    }

    private SslTokenType DetermineTokenType( string token )
    {
      _tokenValue = null;

      switch ( token )
      {
        case "=": return SslTokenType.Assignment;
        case ",": return SslTokenType.Separator;
        case ".": return SslTokenType.Accessor;
        case "[": return SslTokenType.StartArray;
        case "]": return SslTokenType.EndArray;
        case "{":
        {
          ASSERT(
            TokenType == SslTokenType.Identifier ||
            TokenType == SslTokenType.Assignment ||
            TokenType == SslTokenType.StartArray
            );
          _scopeStack.Push( SslTokenType.StartObject );
          return SslTokenType.StartObject;
        }
        case "}":
        {
          ASSERT( _scopeStack.Count > 0 );
          _scopeStack.Pop();
          return SslTokenType.EndObject;
        }
      }

      if ( TryParseStringToken( token, out _tokenValue ) ) return SslTokenType.String;
      if ( TryParseIntegerToken( token, out _tokenValue ) ) return SslTokenType.Integer;
      if ( TryParseDoubleToken( token, out _tokenValue ) ) return SslTokenType.Double;
      if ( TryParseBooleanToken( token, out _tokenValue ) ) return SslTokenType.Boolean;

      return SslTokenType.Identifier;
    }

    private bool TryParseStringToken( string token, out dynamic value )
    {
      value = null;

      if ( token[ 0 ] != '"' )
        return false;

      value = token.Substring( 1, token.Length - 2 );
      return true;
    }

    private bool TryParseIntegerToken( string token, out dynamic value )
    {
      value = null;

      if ( !int.TryParse( token, out var intValue ) )
        return false;

      value = intValue;
      return true;
    }

    private bool TryParseDoubleToken( string token, out dynamic value )
    {
      value = null;

      if ( !double.TryParse( token, out var doubleValue ) )
        return false;

      value = doubleValue;
      return true;
    }

    private bool TryParseBooleanToken( string token, out dynamic value )
    {
      value = null;

      if ( !bool.TryParse( token, out var boolValue ) )
        return false;

      value = boolValue;
      return true;
    }

    private string ReadSslScripting()
    {
      var sb = new StringBuilder();

      while ( !_reader.EndOfStream )
      {
        var line = _reader.ReadLine().Trim();
        if ( line.EndsWith( ">>>" ) || line.EndsWith( "#ssl_end" ) )
          break;

        sb.AppendLine( line );
      }

      return sb.ToString();
    }

    [DebuggerHidden]
    [MethodImpl( MethodImplOptions.NoInlining )]
    private static void ThrowTokenParseFailedException()
      => throw new Exception( "Failed to parse next token." );

    #endregion

  }

}
