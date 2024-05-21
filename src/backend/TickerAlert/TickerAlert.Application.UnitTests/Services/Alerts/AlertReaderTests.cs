using AutoFixture;
using FluentAssertions;
using Moq;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.Services.Prices;
using TickerAlert.Application.UnitTests.Common.Persistence;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.UnitTests.Services.Alerts;

public class AlertReaderTests
{
    private readonly IApplicationDbContext _context;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly AlertReader _alertReader;
    private readonly Fixture _fixture;

    public AlertReaderTests()
    {
        _context = DbContextInMemory.Create();
        _alertReader = new AlertReader(
            _context,
            new PriceMeasureReader(_context),
            CreateMockCurrentUser().Object
        );
        
        _fixture = new Fixture();
    }

    private Mock<ICurrentUserService> CreateMockCurrentUser()
    {
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        mockCurrentUserService.Setup(u => u.UserId).Returns(_userId);
        mockCurrentUserService.Setup(u => u.IsAuthenticated).Returns(true);
        return mockCurrentUserService;
    }

    [Fact]
    public async Task GetAlerts_NoAlerts_ShouldReturnsEmptyList()
    {
        // Arrange. 
        // Empty context state.
        
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
        await InitializeDatabaseWith(alerts);

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
        var priceMeasures = alerts
            .Select(a => PriceMeasure.Create(Guid.NewGuid(), a.FinancialAssetId, _fixture.Create<decimal>()))
            .ToList();

        await InitializeDatabaseWith(alerts, priceMeasures);

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
    
    private async Task InitializeDatabaseWith(List<Alert> alerts, List<PriceMeasure>? priceMeasures = null)
    {
        _context.Alerts.AddRange(alerts);
        _context.PriceMeasures.AddRange(priceMeasures ??= new List<PriceMeasure>());
        await _context.SaveChangesAsync();
    }
}