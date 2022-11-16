using Domain.Todos.Aggregates;

namespace Domain.Todos.Queries;

public record struct TodoGetAllQuery : IQuery<IEnumerable<Todo>>;