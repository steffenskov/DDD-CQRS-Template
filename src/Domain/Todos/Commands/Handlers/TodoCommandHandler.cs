using Domain.Todos.Aggregates;
using Domain.Todos.Repositories;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Commands.Handlers;

internal class TodoCommandHandler
	: IRequestHandler<TodoCreateCommand, Todo>,
		IRequestHandler<TodoUpdateCommand, Todo>,
		IRequestHandler<TodoUpdateDueDateCommand, Todo>,
		IRequestHandler<TodoDeleteCommand, Todo>
{
	private readonly IMediator _mediator;
	private readonly ITodoRepository _repository;

	public TodoCommandHandler(ITodoRepository snapshotRepository, IMediator mediator)
	{
		_repository = snapshotRepository;
		_mediator = mediator;
	}

	public async Task<Todo> Handle(TodoCreateCommand request, CancellationToken cancellationToken)
	{
		var todo = new Todo().With(request); // Here we're creating a new aggregate, and thus cannot use the GetAndApplyCommandAsync method.

		await PersistAggregateAsync(todo, cancellationToken);

		return todo;
	}

	public async Task<Todo> Handle(TodoDeleteCommand request, CancellationToken cancellationToken)
	{
		return await GetAndApplyCommandAsync(request, todo => todo.With(request), cancellationToken);
	}

	public async Task<Todo> Handle(TodoUpdateCommand request, CancellationToken cancellationToken)
	{
		return await GetAndApplyCommandAsync(request, todo => todo.With(request), cancellationToken);
	}

	public async Task<Todo> Handle(TodoUpdateDueDateCommand request, CancellationToken cancellationToken)
	{
		return await GetAndApplyCommandAsync(request, todo => todo.With(request), cancellationToken);
	}

	/// <Summary>
	///     This method encapsulates the process of getting an existing aggregate, applying a command and then finally
	///     persisting.
	///     It's reusable for all command handling that follows that process.
	/// </Summary>
	private async Task<Todo> GetAndApplyCommandAsync(ITodoCommand request, Func<Todo, Todo> modifyAction,
		CancellationToken cancellationToken)
	{
		var current = await GetTodoAsync(request.AggregateId, cancellationToken);

		var modified = modifyAction.Invoke(current);

		await PersistAggregateAsync(modified, cancellationToken);
		return modified;
	}

	private async Task<Todo> GetTodoAsync(TodoId id, CancellationToken cancellationToken)
	{
		return await _repository.GetAsync(id, cancellationToken).ConfigureAwait(false) ??
		       throw new AggregateNotFoundException<Todo, TodoId>(id);
	}

	private async Task PersistAggregateAsync(Todo aggregate, CancellationToken cancellationToken)
	{
		await _repository.SaveAggregateAsync(aggregate, cancellationToken);
	}
}