using System.IO;
using System.Text;
using Index.Modules.Logging.Logging;
using Prism.Events;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Index.Modules.Logging
{

  public class PrismEventSink : ILogEventSink
  {

    #region Data Members

    private readonly IEventAggregator _eventAggregator;
    private readonly ITextFormatter _textFormatter;

    private readonly PubSubEvent<LogMessage> _event;

    #endregion

    public PrismEventSink( IEventAggregator eventAggregator, ITextFormatter textFormatter )
    {
      _eventAggregator = eventAggregator;
      _event = _eventAggregator.GetEvent<LogMessageEvent>();

      _textFormatter = textFormatter;
    }

    public void Emit( LogEvent logEvent )
    {
      if ( logEvent is null )
        return;

      var sourceName = "Global";
      if ( logEvent.Properties.TryGetValue( "SourceContext", out var sourceContextProperty ) )
      {
        sourceName = sourceContextProperty.ToString()
          .Replace( "\"", "" );

        var lastSepIdx = sourceName.LastIndexOf( '.' );
        if ( lastSepIdx != -1 )
          sourceName = sourceName.Substring( lastSepIdx + 1 );
      }

      var message = new LogMessage
      {
        Timestamp = logEvent.Timestamp,
        Level = logEvent.Level,
        Source = sourceName,
        Message = logEvent.RenderMessage(),
        Exception = logEvent.Exception
      };

      _event.Publish( message );

      //var builder = new StringBuilder();
      //var writer = new StringWriter( builder );
      //_textFormatter.Format( logEvent, writer );

      //_event.Publish( builder.ToString() );
    }

  }

}
