using Microsoft.Extensions.Logging;
using SecurityApi.Application.Common.Interfaces;
using SecurityApi.Domain.Common;

namespace SecurityApi.Infrastructure.Services;

public class DomainEventService : IDomainEventService
{
    private readonly ILogger<DomainEventService> _logger;

    public DomainEventService(ILogger<DomainEventService> logger)
    {
        _logger = logger;
    }

    public Task Publish(DomainEvent domainEvent)
    {
        // publish
        throw new NotImplementedException();
    }

}
