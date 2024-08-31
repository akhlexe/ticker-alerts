namespace TickerAlert.Application.Common.Mailing.Dtos;

public record Email(string To, string Subject);

public record AlertTriggeredData(string Company, string Ticker, decimal TargetPrice, string Message, string TriggeredAt);

public record AlertTriggeredEmail(Email Email, AlertTriggeredData Data);


