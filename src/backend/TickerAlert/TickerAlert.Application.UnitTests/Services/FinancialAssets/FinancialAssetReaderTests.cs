using System.Collections.ObjectModel;
using FluentAssertions;
using Moq;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.UnitTests.Services.FinancialAssets;

public class FinancialAssetReaderTests
{
    private readonly Mock<IFinancialAssetRepository> _mockRepository;
    private readonly FinancialAssetReader _financialAssetReader;

    public FinancialAssetReaderTests()
    {
        _mockRepository = new Mock<IFinancialAssetRepository>();
        _financialAssetReader = new FinancialAssetReader(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllBySearchCriteria_ReturnsEmpty_WhenNoAssetsFound()
    {
        // Arrange
        const string criteria = "nonexistent";
        _mockRepository.Setup(repo => repo.GetAllBySearchCriteria(criteria))
                       .ReturnsAsync(Enumerable.Empty<FinancialAsset>());
        // Act
        var results = await _financialAssetReader.GetAllBySearchCriteria(criteria);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllBySearchCriteria_ReturnsAssets_WhenAssetsFound()
    {
        // Arrange
        const string criteria = "example";
        var assets = new List<FinancialAsset>
        {
            new FinancialAsset(1, "ABC", "Example Corp"),
            new FinancialAsset(2, "XYZ", "Example Inc"),
        };
        
        _mockRepository.Setup(repo => repo.GetAllBySearchCriteria(criteria))
                       .ReturnsAsync(assets);

        // Act
        var results = await _financialAssetReader.GetAllBySearchCriteria(criteria);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count());
        Assert.Contains(results, dto => dto is { Ticker: "ABC", Name: "Example Corp" });
        Assert.Contains(results, dto => dto is { Ticker: "XYZ", Name: "Example Inc" });
    }

    [Fact]
    public async Task GetAllBySearchCriteria_ReturnsCorrectData_WhenAssetsFound()
    {
        // Arrange
        const string criteria = "example";
        var assets = new List<FinancialAsset>
        {
            new FinancialAsset(1, "ABC", "Example Corp"),
        };
        _mockRepository.Setup(repo => repo.GetAllBySearchCriteria(criteria))
                       .ReturnsAsync(assets);

        // Act
        var result = await _financialAssetReader.GetAllBySearchCriteria(criteria);

        // Assert
        var assetDto = result.First();
        assetDto.Id.Should().Be(1);
        assetDto.Ticker.Should().Be("ABC");
        assetDto.Name.Should().Be("Example Corp");
    }
}