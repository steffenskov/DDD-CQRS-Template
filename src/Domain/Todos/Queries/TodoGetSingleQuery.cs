using Domain.Todos.Aggregates;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Queries;

public record struct TodoGetSingleQuery(TodoId AggregateId) : IQuery<Todo?>;