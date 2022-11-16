using Dapper.DDD.Repository.Configuration;
using Dapper.DDD.Repository.Repositories;
using Domain.Todos.Aggregates;
using Domain.Todos.ValueObjects;
using Microsoft.Extensions.Options;

namespace Infrastructure.Todos.Repositories;

internal class TodoRepository : TableRepository<Todo, TodoId>, ITodoRepository
{
	public TodoRepository(IOptions<TableAggregateConfiguration<Todo>> options,
		IOptions<DefaultConfiguration> defaultOptions) : base(options, defaultOptions)
	{
	}

	public async Task SaveAggregateAsync(Todo aggregate, CancellationToken cancellationToken)
	{
		if (aggregate.Deleted)
		{
			await DeleteAsync(aggregate.Id, cancellationToken);
		}
		else
		{
			await UpsertAsync(aggregate, cancellationToken);
		}
	}
}