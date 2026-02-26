using Domain.Health;
using Domain.Messages;

namespace Api.Health;

public class HeartbeatService(IMessageProducer messageProducer, ILogger<HeartbeatService> logger) : BackgroundService
{
    private readonly ILogger _logger = logger;
    private readonly Guid _podId = Guid.NewGuid();
    private readonly IMessageProducer _messageProducer = messageProducer;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested)
        {
            await _messageProducer.PublishAsync(Topics.PodHeartbeat, new HealthMessage
            {
                PodId = _podId.ToString(),
                Timestamp = DateTimeOffset.UtcNow
            });
        }
    }
}
