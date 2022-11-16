using Domain.Todos.Aggregates;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Commands;

public interface ITodoCommand : ICommand<Todo, TodoId>
{
}