using Domain.Todos.Aggregates;
using Domain.Todos.Repositories;

namespace Domain.Todos.Queries.Handlers;

internal class TodoQueryHandler
	: IRequestHandler<TodoGetSingleQuery, Todo?>,
		IRequestHandler<TodoGetAllQuery, IEnumerable<Todo>>

{
	// If you don't want to use snapshots to speed up data retrieval, you can replace this with an IEventSourcedRepository
	private readonly ITodoRepository _repository;

	public TodoQueryHandler(ITodoRepository repository)
	{
		_repository = repository;
	}

	public async Task<IEnumerable<Todo>> Handle(TodoGetAllQuery request, CancellationToken cancellationToken)
	{
		return await _repository.GetAllAsync(cancellationToken);
	}

	public async Task<Todo?> Handle(TodoGetSingleQuery request, CancellationToken cancellationToken)
	{
		return await _repository.GetAsync(request.AggregateId, cancellationToken);
	}
}