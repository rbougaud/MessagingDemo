using Serilog.Core;
using Serilog.Events;

namespace Presentation.Filters;

public class ApplicationLogWithRequestsFilter : ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent)
    {
        if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContext))
        {
            // Autorise spécifiquement les logs des requêtes de Entity Framework Core
            if (sourceContext.ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command")
                && logEvent.MessageTemplate.ToString().StartsWith("Executing DbCommand"))
            {
                return true;
            }
            return false;
        }

        return true;
    }
}
