using AutoFixture;
using FluentAssertions;
using Moq;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.UnitTests.Services.Alerts;

public class AlertReaderTests
{
    private readonly Mock<IAlertRepository> _mockAlertRepository;
    private readonly Mock<IPriceMeasureRepository> _mockPriceMeasureRepository;
    private readonly AlertReader _alertReader;
    private readonly Fixture _fixture;

    public AlertReaderTests()
    {
        _mockAlertRepository = new Mock<IAlertRepository>();
        _mockPriceMeasureRepository = new Mock<IPriceMeasureRepository>();
        _alertReader = new AlertReader(_mockAlertRepository.Object, _mockPriceMeasureRepository.Object);
        _fixture = new Fixture();
    }
    
    [Fact]
    public async Task GetAlerts_NoAlerts_ShouldReturnsEmptyList()
    {
        // Arrange
        _mockAlertRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(Enumerable.Empty<Alert>());

        // Act
        var result = await _alertReader.GetAlerts();

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAlerts_WhenAlertsAvailableButNoPriceMeasures_ShouldReturnsAlertsWithZeroActualPrice()
    {
        // Arrange
        var alerts = _fixture.CreateMany<Alert>(3).ToList();
        _mockAlertRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(alerts);
        
        _mockPriceMeasureRepository
            .Setup(repo => repo.GetLastPricesMeasuresFor(It.IsAny<IEnumerable<int>>()))!
            .ReturnsAsync(new List<PriceMeasure>());

        // Act
        var result = await _alertReader.GetAlerts();
    
        // Assert
        Assert.All(result, dto => dto.ActualPrice.Should().Be(0));
    }
    
    [Fact]
    public async Task GetAlerts_AlertsAndPriceMeasuresAvailable_ReturnsCorrectData()
    {
        // Arrange
        var alerts = _fixture.CreateMany<Alert>(3).ToList();
        var priceMeasures =alerts
            .Select(a => new PriceMeasure(a.FinancialAssetId, _fixture.Create<decimal>(), DateTime.UtcNow))
            .ToList();

        _mockAlertRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(alerts);
        
        _mockPriceMeasureRepository
            .Setup(repo => repo.GetLastPricesMeasuresFor(It.IsAny<IEnumerable<int>>()))!
            .ReturnsAsync(priceMeasures);

        // Act
        var result = await _alertReader.GetAlerts();

        // Assert
        foreach (var dto in result)
        {
            var correspondingPriceMeasure = priceMeasures.First(pm => pm.FinancialAssetId == dto.FinancialAssetId);
            Assert.Equal(correspondingPriceMeasure.Price, dto.ActualPrice);
            Assert.Equal(dto.TargetPrice - correspondingPriceMeasure.Price, dto.Difference);
        }
    }
}