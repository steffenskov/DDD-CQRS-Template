namespace Domain.Todos.Commands;

public record TodoCreateCommand(Guid AggregateId, string Title, string Body, DateTime DueDate) : ITodoCommand;
