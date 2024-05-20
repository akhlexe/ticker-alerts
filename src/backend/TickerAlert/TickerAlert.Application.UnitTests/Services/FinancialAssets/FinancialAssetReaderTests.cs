using System.Collections.ObjectModel;
using FluentAssertions;
using Moq;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Application.UnitTests.Common.Persistence;
using TickerAlert.Application.UnitTests.TestData;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.UnitTests.Services.FinancialAssets;

public class FinancialAssetReaderTests
{
    private readonly IApplicationDbContext _context;
    private readonly FinancialAssetReader _financialAssetReader;

    public FinancialAssetReaderTests()
    {
        _context = DbContextInMemory.Create();
        _financialAssetReader = new FinancialAssetReader(_context);
    }

    [Fact]
    public async Task GetAllBySearchCriteria_ReturnsEmpty_WhenNoAssetsFound()
    {
        // Arrange
        const string criteria = "nonexistent";
        
        // Act
        var results = await _financialAssetReader.GetAllBySearchCriteria(criteria);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllBySearchCriteria_ReturnsAssets_WhenAssetsFound()
    {
        // Arrange
        const string criteria = "Example";
        await InitializeDatabaseWith(FinancialAssetsTestData.GetFinancialAssets());

        // Act
        var results = await _financialAssetReader.GetAllBySearchCriteria(criteria);

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(2);
        Assert.Contains(results, dto => dto is { Ticker: "ABC", Name: "Example Corp" });
        Assert.Contains(results, dto => dto is { Ticker: "XYZ", Name: "Example Inc" });
    }

    [Fact]
    public async Task GetAllBySearchCriteria_ReturnsCorrectData_WhenAssetsFound()
    {
        // Arrange
        const string criteria = "Example";
        var newGuid = Guid.NewGuid();
        var assets = new List<FinancialAsset>
        {
            FinancialAsset.Create(newGuid, "ABC", "Example Corp")
        };

        await InitializeDatabaseWith(assets);

        // Act
        var result = await _financialAssetReader.GetAllBySearchCriteria(criteria);

        // Assert
        var assetDto = result.First();
        assetDto.Id.Should().Be(newGuid);
        assetDto.Ticker.Should().Be("ABC");
        assetDto.Name.Should().Be("Example Corp");
    }
    
    private async Task InitializeDatabaseWith(IEnumerable<FinancialAsset> assets)
    {
        _context.FinancialAssets.AddRange(assets);
        await _context.SaveChangesAsync();
    }
}