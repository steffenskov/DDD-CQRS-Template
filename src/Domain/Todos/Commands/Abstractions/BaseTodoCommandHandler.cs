namespace Domain.Todos.Commands.Abstractions;

internal abstract class BaseTodoCommandHandler<T> : IRequestHandler<T, Todo>
where T : BaseTodoCommand
{
	protected readonly IMediator _mediator;
	private readonly ITodoRepository _repository;

	protected BaseTodoCommandHandler(IMediator mediator, ITodoRepository repository)
	{
		_mediator = mediator;
		_repository = repository;
	}

	protected abstract Task<Todo> GetAggregateAsync(T request, CancellationToken cancellationToken);

	public async Task<Todo> Handle(T request, CancellationToken cancellationToken)
	{
		var aggregate = await GetAggregateAsync(request, cancellationToken) ?? throw new AggregateNotFoundException<Todo, TodoId>(request.AggregateId);
		var aggregateToPersist = await request.VisitAsync(aggregate, cancellationToken);
		await _repository.PersistAggregateAsync(aggregateToPersist, cancellationToken); // Consider adding resilience here, using e.g. Polly to ensure both repositories gets persisted
		return aggregateToPersist;
	}
}
