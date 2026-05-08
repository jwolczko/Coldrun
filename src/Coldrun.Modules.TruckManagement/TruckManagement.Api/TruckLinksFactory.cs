using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Api;

public sealed class TruckLinksFactory
{
    public object CreateLinks(string code, string status)
    {
        var links = new Dictionary<string, object>
        {
            ["self"] = new { href = $"/api/v1/trucks/{code}" },
            ["collection"] = new { href = "/api/v1/trucks" },
            ["status"] = new { href = $"/api/v1/trucks/{code}/status" },
            ["delete"] = new { href = $"/api/v1/trucks/{code}" }
        };

        foreach (var nextStatus in GetAllowedStatuses(status))
        {
            var rel = ToRel(nextStatus);
            links[rel] = new
            {
                href = $"/api/v1/trucks/{code}/status",
                title = $"Set status to {nextStatus}"
            };
        }

        return links;
    }

    private static IReadOnlyCollection<string> GetAllowedStatuses(string status)
    {
        return status switch
        {
            "Loading" => ["To Job", "Out Of Service"],
            "To Job" => ["At Job", "Out Of Service"],
            "At Job" => ["Returning", "Out Of Service"],
            "Returning" => ["Loading", "Out Of Service"],
            "Out Of Service" => ["Loading", "To Job", "At Job", "Returning"],
            _ => []
        };
    }

    private static string ToRel(string status)
    {
        return status switch
        {
            "Loading" => "erp:set-status-loading",
            "To Job" => "erp:set-status-to-job",
            "At Job" => "erp:set-status-at-job",
            "Returning" => "erp:set-status-returning",
            "Out Of Service" => "erp:set-out-of-service",
            _ => "erp:unknown"
        };
    }
}
