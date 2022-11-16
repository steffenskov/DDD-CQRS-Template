using Domain.Todos.ValueObjects;

namespace Domain.Todos.Commands;

public record struct TodoCreateCommand(TodoId AggregateId, string Title, string Body, DateTime DueDate) : ITodoCommand;