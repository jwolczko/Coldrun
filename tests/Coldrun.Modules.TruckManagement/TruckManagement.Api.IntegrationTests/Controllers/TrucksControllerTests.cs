using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Api.IntegrationTests.Infrastructure;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.DeleteTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;
using Microsoft.Extensions.DependencyInjection;

namespace Coldrun.Modules.TruckManagement.Api.IntegrationTests.Controllers;

public sealed class TrucksControllerTests
{
    [Fact]
    public async Task GetByCode_WhenTruckExists_ReturnsRepresentation()
    {
        using var factory = new TruckManagementApiFactory(services =>
        {
            services.AddSingleton<IQueryHandler<GetTruckQuery, TruckDetailsDto?>>(
                new DelegateQueryHandler<GetTruckQuery, TruckDetailsDto?>((query, _) =>
                    Task.FromResult<TruckDetailsDto?>(new TruckDetailsDto(
                        query.Code,
                        "Truck 1",
                        "Loading",
                        "Ready",
                        ["To Job", "Out Of Service"]))));

            services.AddSingleton<IQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>>(
                new DelegateQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>((_, _) =>
                    Task.FromResult(new PagedResult<TruckListItemDto>([], 1, 20, 0))));

            services.AddSingleton<ICommandHandler<CreateTruckCommand, CreateTruckResult>>(
                new DelegateCommandHandler<CreateTruckCommand, CreateTruckResult>((_, _) =>
                    Task.FromResult(new CreateTruckResult("TRK-001", "Truck 1", "Loading", "Ready", ["To Job"]))));

            services.AddSingleton<ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>>(
                new DelegateCommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>((_, _) =>
                    Task.FromResult(new UpdateTruckDetailsResult("TRK-001", "Truck 1", "Loading", "Ready", ["To Job"]))));

            services.AddSingleton<ICommandHandler<DeleteTruckCommand>>(
                new DelegateCommandHandler<DeleteTruckCommand>((_, _) => Task.CompletedTask));
        });

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/trucks/TRK-001");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("TRK-001", document.RootElement.GetProperty("code").GetString());
        Assert.Equal("Truck 1", document.RootElement.GetProperty("name").GetString());
        Assert.Equal("/api/v1/trucks/TRK-001",
            document.RootElement.GetProperty("links").GetProperty("self").GetProperty("href").GetString());
    }

    [Fact]
    public async Task Search_WhenCalled_ReturnsPagedCollection()
    {
        using var factory = new TruckManagementApiFactory(services =>
        {
            services.AddSingleton<IQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>>(
                new DelegateQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>((query, _) =>
                    Task.FromResult(new PagedResult<TruckListItemDto>(
                        [new TruckListItemDto("TRK-001", "Truck 1", "Loading", "Ready")],
                        query.PageNumber,
                        query.PageSize,
                        1))));

            services.AddSingleton<IQueryHandler<GetTruckQuery, TruckDetailsDto?>>(
                new DelegateQueryHandler<GetTruckQuery, TruckDetailsDto?>((_, _) =>
                    Task.FromResult<TruckDetailsDto?>(null)));

            services.AddSingleton<ICommandHandler<CreateTruckCommand, CreateTruckResult>>(
                new DelegateCommandHandler<CreateTruckCommand, CreateTruckResult>((_, _) =>
                    Task.FromResult(new CreateTruckResult("TRK-001", "Truck 1", "Loading", "Ready", ["To Job"]))));

            services.AddSingleton<ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>>(
                new DelegateCommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>((_, _) =>
                    Task.FromResult(new UpdateTruckDetailsResult("TRK-001", "Truck 1", "Loading", "Ready", ["To Job"]))));

            services.AddSingleton<ICommandHandler<DeleteTruckCommand>>(
                new DelegateCommandHandler<DeleteTruckCommand>((_, _) => Task.CompletedTask));
        });

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/trucks?pageNumber=2&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(2, document.RootElement.GetProperty("pageNumber").GetInt32());
        Assert.Equal(5, document.RootElement.GetProperty("pageSize").GetInt32());
        Assert.Equal(1, document.RootElement.GetProperty("totalElements").GetInt64());
        Assert.Equal("TRK-001", document.RootElement.GetProperty("items")[0].GetProperty("code").GetString());
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ReturnsCreatedResponse()
    {
        using var factory = new TruckManagementApiFactory(services =>
        {
            services.AddSingleton<ICommandHandler<CreateTruckCommand, CreateTruckResult>>(
                new DelegateCommandHandler<CreateTruckCommand, CreateTruckResult>((command, _) =>
                    Task.FromResult(new CreateTruckResult(
                        command.Code,
                        command.Name,
                        command.Status,
                        command.Description,
                        ["To Job", "Out Of Service"]))));

            services.AddSingleton<IQueryHandler<GetTruckQuery, TruckDetailsDto?>>(
                new DelegateQueryHandler<GetTruckQuery, TruckDetailsDto?>((_, _) =>
                    Task.FromResult<TruckDetailsDto?>(null)));

            services.AddSingleton<IQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>>(
                new DelegateQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>((_, _) =>
                    Task.FromResult(new PagedResult<TruckListItemDto>([], 1, 20, 0))));

            services.AddSingleton<ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>>(
                new DelegateCommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>((_, _) =>
                    Task.FromResult(new UpdateTruckDetailsResult("TRK-001", "Truck 1", "Loading", "Ready", ["To Job"]))));

            services.AddSingleton<ICommandHandler<DeleteTruckCommand>>(
                new DelegateCommandHandler<DeleteTruckCommand>((_, _) => Task.CompletedTask));
        });

        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/trucks", new
        {
            code = "TRK-001",
            name = "Truck 1",
            status = "Loading",
            description = "Ready"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("/api/v1/trucks/TRK-001", response.Headers.Location?.OriginalString);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("TRK-001", document.RootElement.GetProperty("code").GetString());
        Assert.Equal("Loading", document.RootElement.GetProperty("status").GetString());
    }
}
