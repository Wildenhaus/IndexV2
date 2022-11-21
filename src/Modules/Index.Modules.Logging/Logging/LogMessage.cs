using System;
using Serilog.Events;

namespace Index.Modules.Logging.Logging
{

  public class LogMessage
  {

    public DateTimeOffset Timestamp { get; set; }
    public LogEventLevel Level { get; set; }
    public string Source { get; set; }
    public string Message { get; set; }
    public Exception Exception { get; set; }

  }

}
