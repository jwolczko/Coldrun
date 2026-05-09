using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Api.IntegrationTests.Infrastructure;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatus;
using Microsoft.Extensions.DependencyInjection;

namespace Coldrun.Modules.TruckManagement.Api.IntegrationTests.Controllers;

public sealed class TruckStatusesControllerTests
{
    [Fact]
    public async Task ChangeStatus_WhenRequestIsValid_ReturnsUpdatedStatusRepresentation()
    {
        using var factory = new TruckManagementApiFactory(services =>
        {
            services.AddSingleton<ICommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult>>(
                new DelegateCommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult>((command, _) =>
                    Task.FromResult(new ChangeTruckStatusResult(
                        command.Code,
                        command.Status,
                        ["At Job", "Out Of Service"]))));

            services.AddSingleton<IQueryHandler<GetTruckStatusQuery, TruckStatusDto?>>(
                new DelegateQueryHandler<GetTruckStatusQuery, TruckStatusDto?>((query, _) =>
                    Task.FromResult<TruckStatusDto?>(new TruckStatusDto(
                        query.Code,
                        "To Job",
                        ["At Job", "Out Of Service"]))));
        });

        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync("/api/v1/trucks/TRK-001/status", new
        {
            status = "To Job"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("TRK-001", document.RootElement.GetProperty("code").GetString());
        Assert.Equal("To Job", document.RootElement.GetProperty("status").GetString());
        Assert.Equal("/api/v1/trucks/TRK-001/status",
            document.RootElement.GetProperty("links").GetProperty("status").GetProperty("href").GetString());
    }
}
