using Domain.Todos.ValueObjects;

namespace Domain.Todos.Commands;

public record struct TodoDeleteCommand(TodoId AggregateId) : ITodoCommand;