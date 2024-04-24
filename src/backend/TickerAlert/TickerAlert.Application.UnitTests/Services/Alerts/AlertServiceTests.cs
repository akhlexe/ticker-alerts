using Moq;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.UnitTests.Services.Alerts;

public class AlertServiceTests
{
    private readonly Mock<IAlertRepository> _mockAlertRepository;
    private readonly Mock<IPriceMeasureRepository> _mockPriceMeasureRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly AlertService _alertService;
    private const int USER_ID = 1;

    public AlertServiceTests()
    {
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockAlertRepository = new Mock<IAlertRepository>();
        _mockPriceMeasureRepository = new Mock<IPriceMeasureRepository>();
        _alertService = new AlertService(
            _mockAlertRepository.Object,
            _mockPriceMeasureRepository.Object,
            _mockCurrentUserService.Object
        );

        _mockCurrentUserService.Setup(x => x.UserId).Returns(USER_ID);
        _mockCurrentUserService.Setup(x => x.IsAuthenticated).Returns(true);
    }
    
    [Fact]
    public async Task CreateAlert_WithValidData_CreatesAlert()
    {
        // Arrange
        const int assetId = 1;
        const int userId = 1;
        const decimal targetPrice = 100m;
        const decimal currentPrice = 90m;
        
        _mockPriceMeasureRepository
            .Setup(repo => repo.GetLastPriceMeasureFor(assetId))
            .ReturnsAsync(new PriceMeasure(assetId, currentPrice, DateTime.UtcNow));

        _mockAlertRepository
            .Setup(repo => repo.CreateAlert(USER_ID, assetId, targetPrice, It.IsAny<PriceThresholdType>()))
            .Returns(Task.CompletedTask);

        // Act
        await _alertService.CreateAlert(assetId, targetPrice);

        // Assert
        _mockAlertRepository.Verify(repo => repo.CreateAlert(USER_ID, assetId, targetPrice, PriceThresholdType.Above), Times.Once);
    }
}