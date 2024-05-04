using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using TickerAlert.Domain.Common;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.Persistence.Outbox;

namespace TickerAlert.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto
    };
    
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;
    
    public ProcessOutboxMessagesJob(
        ApplicationDbContext context, 
        IPublisher publisher, 
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> outboxMessages = await GetBatchOfUnprocessedOutboxMessages(context);

        foreach (var message in outboxMessages)
        {
            var (isSuccess, domainEvent) = TryDeserializeMessage(message.Content);
            if (!isSuccess)
            {
                _logger.LogWarning("Failed to deserialize message with ID: {MessageId}", message.Id);
                continue;
            }

            bool isPublished = await TryPublishEvent(domainEvent, context.CancellationToken);
            if (!isPublished)
            {
                continue;
            }

            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    private async Task<List<OutboxMessage>> GetBatchOfUnprocessedOutboxMessages(IJobExecutionContext context)
    {
        return await _context
            .OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);
    }
    
    private (bool isSuccess, IDomainEvent? domainEvent) TryDeserializeMessage(string content)
    {
        try
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(content, JsonSerializerSettings);
            return (domainEvent != null, domainEvent);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing domain event from JSON.");
            return (false, null);
        }
    }
    
    private async Task<bool> TryPublishEvent(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        try
        {
            await _publisher.Publish(domainEvent, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain event.");
            return false;
        }
    }
}