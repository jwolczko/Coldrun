using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

namespace Coldrun.Modules.TruckManagement.Application.UnitTests.Trucks.Queries.SearchTrucks;

public sealed class SearchTrucksQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenPageSizeIsTooLarge_ThrowsException()
    {
        var readModel = new FakeTruckReadModel();
        var handler = new SearchTrucksQueryHandler(readModel);
        var query = new SearchTrucksQuery(null, null, null, null, null, null, 1, 101);

        await Assert.ThrowsAsync<InvalidPaginationException>(() =>
            handler.HandleAsync(query, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_WhenQueryIsValid_NormalizesCriteriaAndMapsResult()
    {
        var readModel = new FakeTruckReadModel
        {
            SearchAsyncResult = new PagedResult<TruckListItemProjection>(
                [
                    new TruckListItemProjection("TRK-001", "Truck 1", "Loading", "Ready")
                ],
                2,
                5,
                11)
        };

        var handler = new SearchTrucksQueryHandler(readModel);
        var query = new SearchTrucksQuery(
            " TRK-001 ",
            " TRK ",
            " Truck ",
            "loading",
            " Ready ",
            "-name,code",
            2,
            5);

        var result = await handler.HandleAsync(query, CancellationToken.None);

        Assert.NotNull(readModel.LastCriteria);
        Assert.Equal("TRK-001", readModel.LastCriteria!.Code);
        Assert.Equal("TRK", readModel.LastCriteria.CodeContains);
        Assert.Equal("Truck", readModel.LastCriteria.NameContains);
        Assert.Equal("Loading", readModel.LastCriteria.Status);
        Assert.Equal("Ready", readModel.LastCriteria.DescriptionContains);
        Assert.Collection(
            readModel.LastCriteria.Sort,
            field =>
            {
                Assert.Equal("Name", field.Field);
                Assert.Equal(TruckSortDirection.Descending, field.Direction);
            },
            field =>
            {
                Assert.Equal("Code", field.Field);
                Assert.Equal(TruckSortDirection.Ascending, field.Direction);
            });

        Assert.Single(result.Items);
        Assert.Equal("TRK-001", result.Items.Single().Code);
        Assert.Equal(11, result.TotalElements);
        Assert.Equal(3, result.TotalPages);
    }

    private sealed class FakeTruckReadModel : ITruckReadModel
    {
        public SearchTrucksCriteria? LastCriteria { get; private set; }
        public PagedResult<TruckListItemProjection> SearchAsyncResult { get; set; } =
            new([], 1, 10, 0);

        public Task<TruckDetailsProjection?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<TruckDetailsProjection?>(null);
        }

        public Task<TruckStatusProjection?> GetStatusByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<TruckStatusProjection?>(null);
        }

        public Task<PagedResult<TruckListItemProjection>> SearchAsync(
            SearchTrucksCriteria criteria,
            CancellationToken cancellationToken = default)
        {
            LastCriteria = criteria;
            return Task.FromResult(SearchAsyncResult);
        }
    }
}
