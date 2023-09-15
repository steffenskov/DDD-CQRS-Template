
namespace Domain.Todos.Commands.Abstractions;

public abstract record BaseTodoCommand(TodoId AggregateId) : ICommand<Todo, TodoId>
{
	public abstract Task<Todo> VisitAsync(Todo aggregate, CancellationToken cancellationToken);
}
