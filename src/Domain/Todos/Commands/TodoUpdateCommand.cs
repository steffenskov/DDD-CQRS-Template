using Domain.Todos.ValueObjects;

namespace Domain.Todos.Commands;

public record struct TodoUpdateCommand(TodoId AggregateId, string Title, string Body, DateTime DueDate) : ITodoCommand;