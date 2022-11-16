namespace Domain.Abstractions.Aggregates;

public interface IAggregate<out TAggregateId>
{
	TAggregateId Id { get; }
}