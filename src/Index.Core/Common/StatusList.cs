using System.Collections;

namespace Index
{

  public class StatusList : IEnumerable<StatusList.Entry>
  {

    #region Data Members

    private readonly object _lock;

    private readonly List<Entry> _messages;
    private readonly List<Entry> _warnings;
    private readonly List<Entry> _errors;

    #endregion

    #region Properties

    public IReadOnlyList<Entry> Messages => _messages;
    public IReadOnlyList<Entry> Warnings => _warnings;
    public IReadOnlyList<Entry> Errors => _errors;

    public int Count => _messages.Count + _warnings.Count + _errors.Count;

    public bool HasMessages => _messages.Count > 0;
    public bool HasWarnings => _warnings.Count > 0;
    public bool HasErrors => _errors.Count > 0;

    #endregion

    #region Constructor

    public StatusList()
    {
      _lock = new object();

      _messages = new List<Entry>();
      _warnings = new List<Entry>();
      _errors = new List<Entry>();
    }

    #endregion

    #region Public Methods

    public void AddMessage( string name, string messageFormat, params object[] formatArgs )
    {
      var message = string.Format( messageFormat, formatArgs );
      var entry = new Entry( StatusListEntryType.Message, name, message );

      lock ( _lock )
        _messages.Add( entry );
    }

    public void AddWarning( string name, string messageFormat, params object[] formatArgs )
    {
      var message = string.Format( messageFormat, formatArgs );
      var entry = new Entry( StatusListEntryType.Warning, name, message );

      lock ( _lock )
        _warnings.Add( entry );
    }

    public void AddError( string name, string messageFormat, params object[] formatArgs )
    {
      var message = string.Format( messageFormat, formatArgs );
      var entry = new Entry( StatusListEntryType.Error, name, message );

      lock ( _lock )
        _errors.Add( entry );
    }

    public void AddError( string name, Exception exception )
    {
      var entry = new Entry( StatusListEntryType.Error, name, exception: exception );

      lock ( _lock )
        _errors.Add( entry );
    }

    public void AddError( string name, Exception exception, string messageFormat, params object[] formatArgs )
    {
      var message = string.Format( messageFormat, formatArgs );
      var entry = new Entry( StatusListEntryType.Error, name, message, exception: exception );

      lock ( _lock )
        _errors.Add( entry );
    }

    public void Merge( StatusList statusListToMerge )
    {
      lock ( _lock )
      {
        _messages.AddRange( statusListToMerge.Messages );
        _warnings.AddRange( statusListToMerge.Warnings );
        _errors.AddRange( statusListToMerge.Errors );
      }
    }

    #endregion

    #region IEnumerable Methods

    public IEnumerator<Entry> GetEnumerator()
    {
      lock ( _lock )
      {
        var entries = _messages.Concat( _warnings ).Concat( _errors );
        foreach ( var entry in entries.OrderBy( x => x.Time ) )
          yield return entry;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();

    #endregion

    #region Embedded Types

    public readonly struct Entry
    {

      #region Data Members

      public readonly DateTime Time;
      public readonly StatusListEntryType Type;
      public readonly string Name;
      public readonly string Message;
      public readonly Exception Exception;

      #endregion

      #region Constructor

      public Entry( StatusListEntryType type, string name, string message = null, Exception exception = null )
      {
        Time = DateTime.Now;
        Type = type;
        Name = name;
        Message = message ?? exception?.Message;
        Exception = exception;
      }

      #endregion

    }

    #endregion

  }

  public enum StatusListEntryType
  {
    Message = 1,
    Warning = 2,
    Error = 3
  }

}
