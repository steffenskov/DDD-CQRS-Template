using Domain.Todos.Queries;

namespace Domain.Todos.Commands.Abstractions;

internal abstract class BaseTodoUpdateCommandHandler<T> : BaseTodoCommandHandler<T>
where T : BaseTodoCommand
{
	protected BaseTodoUpdateCommandHandler(IMediator mediator, ITodoRepository repository) : base(mediator, repository)
	{
	}

	protected override async Task<Todo> GetAggregateAsync(T request, CancellationToken cancellationToken)
	{
		return await _mediator.Send(new TodoGetSingleQuery(request.AggregateId), cancellationToken) ?? throw new AggregateNotFoundException<Todo, TodoId>(request.AggregateId);
	}
}
