namespace Domain.Abstractions.Repositories;

public interface IRepository<TAggregate, TAggregateId>
where TAggregate : IAggregate<TAggregateId>
where TAggregateId : StrongTypedGuid<TAggregateId>
{
	Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken);
	Task<TAggregate?> GetAsync(TAggregateId id, CancellationToken cancellationToken);
	Task PersistAggregateAsync(TAggregate aggregate, CancellationToken cancellationToken);
}
