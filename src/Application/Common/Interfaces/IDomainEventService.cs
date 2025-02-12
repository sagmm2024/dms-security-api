using SecurityApi.Domain.Common;

namespace SecurityApi.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
