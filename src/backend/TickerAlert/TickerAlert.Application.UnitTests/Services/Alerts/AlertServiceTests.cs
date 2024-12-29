using FluentAssertions;
using Moq;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.UnitTests.Common.Persistence;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.UnitTests.Services.Alerts;

public class AlertServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly Mock<IPriceMeasureReader> _priceMeasureReader;
    private readonly AlertService _alertService;
    private readonly Guid _userId = Guid.NewGuid();

    public AlertServiceTests()
    {
        _context = DbContextInMemory.Create();
        _priceMeasureReader = new Mock<IPriceMeasureReader>();
        _alertService = new AlertService(
            _context,
            _priceMeasureReader.Object,
            CreateCurrentUserServiceMock().Object
        );

        _priceMeasureReader
            .Setup(x => x.GetLastPriceFor(It.IsAny<Guid>()))
            .ReturnsAsync(0);
    }

    private Mock<ICurrentUserService> CreateCurrentUserServiceMock()
    {
        Mock<ICurrentUserService> mockCurrentUserService = new();
        mockCurrentUserService.Setup(x => x.UserId).Returns(_userId);
        mockCurrentUserService.Setup(x => x.IsAuthenticated).Returns(true);
        return mockCurrentUserService;
    }

    [Fact]
    public async Task CreateAlert_WithValidData_CreatesAlert()
    {
        // Arrange
        Guid assetId = Guid.NewGuid();
        const decimal targetPrice = 100m;
        const decimal currentPrice = 90m;

        _priceMeasureReader
            .Setup(x => x.GetLastPriceFor(It.IsAny<Guid>()))
            .ReturnsAsync(currentPrice);

        // Act
        await _alertService.CreateAlert(assetId, targetPrice);

        // Assert
        var alert = _context.Alerts.First();
        alert.PriceThreshold.Should().Be(PriceThresholdType.Above);
        alert.FinancialAssetId.Should().Be(assetId);
        alert.TargetPrice.Should().Be(targetPrice);
    }

    [Fact]
    public async Task TriggerAlert()
    {
        // Arrange
        var alert = CreatePendingAlert();
        alert.State.Should().Be(AlertState.PENDING);
        
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();

        // Act
        await _alertService.TriggerAlert(alert);

        // Assert
        var first = _context.Alerts.First();
        first.State.Should().Be(AlertState.TRIGGERED);
    }
    
    [Fact]
    public async Task NotifyAlert()
    {
        // Arrange
        var alert = CreateTriggeredAlert();
        alert.State.Should().Be(AlertState.TRIGGERED);
        
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();

        // Act
        await _alertService.NotifyAlert(alert);

        // Assert
        var first = _context.Alerts.First();
        first.State.Should().Be(AlertState.NOTIFIED);
    }

    private Alert CreatePendingAlert()
    {
        return Alert.Create(Guid.NewGuid(), _userId, Guid.NewGuid(), 100, PriceThresholdType.Above);
    }
    
    private Alert CreateTriggeredAlert()
    {
        var alert = Alert.Create(Guid.NewGuid(), _userId, Guid.NewGuid(), 100, PriceThresholdType.Above);
        alert.State = AlertState.TRIGGERED;

        return alert;
    }
}