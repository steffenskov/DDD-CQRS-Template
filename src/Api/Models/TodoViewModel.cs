using Domain.Todos.Aggregates;
using Domain.Todos.ValueObjects;

namespace Api.Models;

public class TodoViewModel
{
	public TodoId Id { get; }
	public string Title { get; }

	public string Body { get; }

	public DateTime DueDate { get; }

	public TodoViewModel(Todo todo) // We could also use AutoMapper instead of manually mapping
	{
		Id = todo.Id;
		Title = todo.Title;
		Body = todo.Body;
		DueDate = todo.DueDate;
	}
}