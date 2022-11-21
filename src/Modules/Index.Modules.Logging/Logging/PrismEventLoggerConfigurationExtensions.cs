using System;
using Prism.Events;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Index.Modules.Logging.Logging
{

  public static class PrismEventLoggerConfigurationExtensions
  {

    #region Constants

    const string DefaultOutputTemplate = "{Timestamp:hh:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

    #endregion

    #region Public Methods

    public static LoggerConfiguration PrismEvent(
            this LoggerSinkConfiguration sinkConfiguration,
            IEventAggregator eventAggregator,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null )
    {
      ASSERT_NOT_NULL( eventAggregator );
      ASSERT_NOT_NULL( outputTemplate );

      var formatter = new MessageTemplateTextFormatter( outputTemplate, formatProvider );
      var sink = new PrismEventSink( eventAggregator, formatter );
      return sinkConfiguration.Sink( sink, restrictedToMinimumLevel, levelSwitch );
    }

    #endregion

  }

}
