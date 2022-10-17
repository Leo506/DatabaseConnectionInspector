using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Core;

public class StringConstants
{
    public const string NoConnectionsProvided =
        "No database connections was provided so check has automatically passed";

    public const string ConnectionFailed = "Database connection {0} inform about losing connection";

    public const string NoRequireInspection = "This endpoint does not require a database connection check";
    public const string NoEndpoint = "Failed to extract endpoint data from current request";
}