using AutoFixture;
using FluentAssertions;
using Moq;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.Services.Prices;
using TickerAlert.Application.UnitTests.Common.Persistence;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.UnitTests.Services.Alerts;

public class AlertServiceTests
{
    private readonly IApplicationDbContext _context;
    private readonly AlertService _alertService;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Fixture _fixture;

    public AlertServiceTests()
    {
        _context = DbContextInMemory.Create();
        _fixture = new Fixture();
        _alertService = new AlertService(
            _context,
            new PriceMeasureReader(_context),
            CreateCurrentUserServiceMock().Object
        );
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
        
        var priceMeasure = PriceMeasure.Create(Guid.NewGuid(), assetId, currentPrice);
        _context.PriceMeasures.Add(priceMeasure);
        await _context.SaveChangesAsync();

        // Act
        await _alertService.CreateAlert(assetId, targetPrice);

        // Assert
        var alert = _context.Alerts.First();
        alert.PriceThreshold.Should().Be(PriceThresholdType.Above);
        alert.FinancialAssetId.Should().Be(assetId);
        alert.TargetPrice.Should().Be(targetPrice);
    }
}