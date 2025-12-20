using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Manager.Core.Logging.Configuration;

public class CustomWriteStrategy
{
    public virtual LoggerConfiguration Apply(LoggerSinkConfiguration writeTo) =>
        writeTo.Sink(new StubSink(), LevelAlias.Off);

    private class StubSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent) { }
    }
}