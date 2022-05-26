using Domain.Todos.Aggregates;
using Domain.Todos.Repositories;

namespace Domain.Todos.Commands.Handlers;

internal class TodoCommandHandler
	: IRequestHandler<TodoCreateCommand, Todo>,
	IRequestHandler<TodoUpdateCommand, Todo>,
	IRequestHandler<TodoUpdateDueDateCommand, Todo>,
	IRequestHandler<TodoDeleteCommand, Todo>
{
	private readonly ITodoRepository _repository;
	private readonly IMediator _mediator;

	public TodoCommandHandler(ITodoRepository snapshotRepository, IMediator mediator)
	{
		_repository = snapshotRepository;
		_mediator = mediator;
	}

	public async Task<Todo> Handle(TodoCreateCommand request, CancellationToken cancellationToken)
	{
		var todo = new Todo(); // Here we're creating a new aggregate, and thus cannot use the GetAndApplyCommandAsync method.

		todo.When(request);

		await PersistAggregateAsync(todo, cancellationToken).ConfigureAwait(false);

		return todo;
	}

	public async Task<Todo> Handle(TodoUpdateCommand request, CancellationToken cancellationToken)
	{
		return await GetAndApplyCommandAsync(request, todo => todo.When(request), cancellationToken).ConfigureAwait(false);
	}

	public async Task<Todo> Handle(TodoUpdateDueDateCommand request, CancellationToken cancellationToken)
	{
		return await GetAndApplyCommandAsync(request, todo => todo.When(request), cancellationToken).ConfigureAwait(false);
	}

	public async Task<Todo> Handle(TodoDeleteCommand request, CancellationToken cancellationToken)
	{
		return await GetAndApplyCommandAsync(request, todo => todo.When(request), cancellationToken).ConfigureAwait(false);
	}

	/// <Summary>
	/// This method encapsulates the process of getting an existing aggregate, applying a command and then finally persisting.
	/// It's reusable for all command handling that follows that process.
	/// </Summary>
	private async Task<Todo> GetAndApplyCommandAsync(ITodoCommand request, Action<Todo> modifyAction, CancellationToken cancellationToken)
	{
		var todo = await GetTodoAsync(request.AggregateId, cancellationToken).ConfigureAwait(false);

		modifyAction.Invoke(todo);

		await PersistAggregateAsync(todo, cancellationToken).ConfigureAwait(false);
		return todo;
	}

	private async Task<Todo> GetTodoAsync(Guid id, CancellationToken cancellationToken)
	{
		return await _repository.GetAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new AggregateNotFoundException<Todo, Guid>(id);
	}

	private async Task PersistAggregateAsync(Todo aggregate, CancellationToken cancellationToken)
	{
		await _repository.SaveAggregateAsync(aggregate, cancellationToken).ConfigureAwait(false);
	}
}
