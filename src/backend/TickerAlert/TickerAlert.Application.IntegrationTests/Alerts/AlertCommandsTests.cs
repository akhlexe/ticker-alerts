using TickerAlert.Application.IntegrationTests.Common;
using TickerAlert.Application.UseCases.Alerts.CreateAlert;

namespace TickerAlert.Application.IntegrationTests.Alerts;

public class AlertCommandsTests : BaseIntegrationTest
{
    public AlertCommandsTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AlertCanBeCreated()
    {
        // Arrange
        var financialAsset = DbContext.FinancialAssets.FirstOrDefault();
        var command = new CreateAlertCommand(financialAsset.Id, 1000);

        // Act
        var result = await Sender.Send(command);

        // Assert
        var alert = DbContext.Alerts.FirstOrDefault(x => x.Id == result.Data);
        Assert.NotNull(alert);
    }
}