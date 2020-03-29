---
layout: default
title: Logging
parent: Documentation
nav_order: 6
has_children: false
---

# Logging

Chesster offers a simple logging implementation, similar to other applications. Log messages are categorized by their severity level, and can include a date, time, sender type, message and additional information.

| Level | Description |
|:------|:------------|
| Debug | A piece of data that can be useful to figure out what variables are in use. |
| Info | An information regarding the current action of the library. |
| Warn | Something that could potentially cause unexpected behaviour. |
| Error | Something that went wrong, but can be handled. |
| Fatal | Something that went terribly wrong, and can lead to a crash or data loss. |

## Setting up

To allow logs to be displayed, you need to register a log output source. By default, Chesster can output to a file and the console.

```cs
using Chesster.Logging;

Logger.RegisterOutput<ConsoleLogOutput>();
Logger.RegisterOutput<FileLogOutput>();
```

Other kinds of settings can be configured via `Logger.Settings`.

| Property | Type | Description |
|:---------|:-----|:------------|
| IncludeDate | Boolean | Should the date be included in the log message? `False` by default. |
| IncludeTime | Boolean | Should the time be included in the log message? `True` by default. |
| DateFormat | String | If the date should be included, this string specifies the format. `yyyy/MM/dd` by default. |
| TimeFormat | String | If the time should be included, this string specifies the format. `HH:mm:ss` by default. |
| MinimumLevel | LogLevel | The minimum log level that should be logged. Lower-level messages will be ignored. `Error` by default. |

## Logging Messages

If you are extending a part of the library, or just want a convenient logging service, you can simply call the static `Logger` class, which provides various methods to easily create log messages. Logs can include a type, to allow a better understanding where log messages arose from.

```cs
// This message will include a sender type.
Logger.Debug<MyAwesomeType>("My awesome log message!");
// This message won't!
Logger.Error("Oh no, something went wrong!");

// This can be used to have more control over log message properties:
Logger.Append(new LogMessage() { ... });

// This can be used to log exceptions.
// Exceptions are automatically categorized as 'Errors' and include a stacktrace.
try { /* Some code */ }
catch (Exception e)
{
    Logger.Exception(e);
}
```

## Customization

### Log Outputs

If you need to add another kind of log output (to a database for example) you simply have a type inherit the `LogOutput` base class. The only thing the inheriting class is required to have, is a `Log` method with a `LogMessage` and a `LoggerSettings` parameter.

```cs
public class MyLogOutput : LogOutput
{
    public override void Log(LogMessage message, LoggerSettings settings)
    {
        // My awesome log code here!
    }
}
```