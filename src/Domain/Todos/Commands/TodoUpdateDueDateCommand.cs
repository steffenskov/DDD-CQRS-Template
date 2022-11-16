using Domain.Todos.ValueObjects;

namespace Domain.Todos.Commands;

public record struct TodoUpdateDueDateCommand(TodoId AggregateId, DateTime DueDate) : ITodoCommand;