using Domain.Todos.Aggregates;

namespace Domain.Todos.Commands;

public record TodoDeleteCommand(Guid AggregateId) : ITodoCommand;