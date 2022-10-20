using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Core;

/// <summary>
/// Class containing messages for logging
/// </summary>
public static class StringConstants  // TODO rename class to LoggingMessages
{
    public const string NoConnectionsProvided =
        "No database connections was provided so check has automatically passed";

    public const string ConnectionFailed = "Database connection {0} inform about losing connection";

    public const string NoRequireInspection = "This endpoint does not require a database connection check";
}