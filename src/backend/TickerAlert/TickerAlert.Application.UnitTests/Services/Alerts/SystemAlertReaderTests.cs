using AutoFixture;
using FluentAssertions;
using TickerAlert.Application.Common.Persistence;
using TickerAlert.Application.Services.Alerts;
using TickerAlert.Application.UnitTests.Common.Persistence;
using TickerAlert.Domain.Entities;
using TickerAlert.Domain.Enums;

namespace TickerAlert.Application.UnitTests.Services.Alerts;

public class SystemAlertReaderTests
{
    public readonly SystemAlertReader _sut;
    public readonly IApplicationDbContext _context;
    
    public SystemAlertReaderTests()
    {
        _context = DbContextInMemory.Create();
        _sut = new SystemAlertReader(_context);
    }
    
    [Fact]
    public async Task GetAllWithPendingStateAndByFinancialAssetId_ReturnsPendingAlerts_ForGivenFinancialAssetId()
    {
        // Arrange
        var financialAssetId = Guid.NewGuid();
        var pendingAlert = CreatePendingAlert(financialAssetId);
        var pendingAlert2 = CreatePendingAlert(Guid.NewGuid());
        var triggeredAlert = CreateTriggeredAlert(financialAssetId);
        
        _context.Alerts.AddRange(pendingAlert, pendingAlert2, triggeredAlert);
        await _context.SaveChangesAsync();
        
        // Act
        var alerts = await _sut.GetPendingAlertsByFinancialAsset(financialAssetId);

        // Assert
        alerts.Should().NotBeNull();
        alerts.Should().ContainSingle();
        alerts.First().Should().Match<Alert>(alert => 
            alert.FinancialAssetId == financialAssetId && alert.State == AlertState.PENDING);
    }

    [Fact]
    public async Task GetPendingAlertsByFinancialAsset_WhenNotMatch_ShouldReturnEmptyList()
    {
        // Arrange
        var financialAssetId = Guid.NewGuid(); 
        
        // Act
        var alerts = await _sut.GetPendingAlertsByFinancialAsset(financialAssetId);

        // Assert
        alerts.Should().BeEmpty();
    }
    
    private Alert CreateTriggeredAlert(Guid financialAssetId)
    {
        var alert = Alert.Create(Guid.NewGuid(), Guid.NewGuid(), financialAssetId, 100, PriceThresholdType.Below);
        alert.State = AlertState.TRIGGERED;
        return alert;
    }

    private static Alert CreatePendingAlert(Guid financialAssetId)
    {
        return Alert.Create(Guid.NewGuid(), Guid.NewGuid(), financialAssetId, 100, PriceThresholdType.Below);
    }
}