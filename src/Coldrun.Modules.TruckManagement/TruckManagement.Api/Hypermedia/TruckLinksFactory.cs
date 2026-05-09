namespace Coldrun.Modules.TruckManagement.Api.Hypermedia;

public sealed class TruckLinksFactory
{
    public IReadOnlyDictionary<string, object> CreateForTruck(
        string code,
        IReadOnlyCollection<string> allowedStatusTransitions)
    {
        var links = new Dictionary<string, object>
        {
            ["self"] = new
            {
                href = $"/api/v1/trucks/{code}"
            },
            ["collection"] = new
            {
                href = "/api/v1/trucks"
            },
            ["status"] = new
            {
                href = $"/api/v1/trucks/{code}/status"
            },
            ["update"] = new
            {
                href = $"/api/v1/trucks/{code}",
                method = "PATCH"
            },
            ["delete"] = new
            {
                href = $"/api/v1/trucks/{code}",
                method = "DELETE"
            }
        };

        foreach (var status in allowedStatusTransitions)
        {
            links[ToRelationName(status)] = new
            {
                href = $"/api/v1/trucks/{code}/status",
                method = "PUT",
                title = $"Set status to {status}"
            };
        }

        return links;
    }

    private static string ToRelationName(string status)
    {
        return status switch
        {
            "Loading" => "erp:set-status-loading",
            "To Job" => "erp:set-status-to-job",
            "At Job" => "erp:set-status-at-job",
            "Returning" => "erp:set-status-returning",
            "Out Of Service" => "erp:set-out-of-service",
            _ => "erp:unknown-status-transition"
        };
    }
}
