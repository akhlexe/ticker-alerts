using TickerAlert.Infrastructure.EventBus.Extensions;

namespace TickerAlert.Infrastructure.EventBus.Definitions;

public static class RabbitMqDefinitions
{
    public static IReadOnlyList<RabbitMqBindingDefinition> GetBindings()
    {
        return new List<RabbitMqBindingDefinition>
        {
            new RabbitMqBindingDefinition("ticker.price", "ticker.price.updates", "price.update"),
            new RabbitMqBindingDefinition("ticker.alert", "ticker.alert.triggered", "alert.triggered"),
            //new BindingDefinition("ticker.alert", "ticker.alert.notification", "notifi"),
            // Add more bindings as needed
        };
    }
}
