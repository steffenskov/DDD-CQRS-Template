using Domain.Todos.Commands;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Aggregates;

// The aggregate class is public, but all command handling is kept internal to force the developer to go through CQRS.
public class Todo : IAggregate<TodoId>
{
	public string Title { get; private set; } = default!;
	public string Body { get; private set; } = default!;
	public DateTime DueDate { get; private set; }
	public bool Deleted { get; private set; }
	public TodoId Id { get; private set; } = default!;

	internal void When(TodoCreateCommand command)
	{
		// Validate ALL domain rules relevant for this event, prior to setting any properties to avoid an aggregate stuck in limbo.
		ValidateBody(command.Body);
		ValidateTitle(command.Title);
		ValidateDueDate(command.DueDate);

		Deleted = false; // A new todo is obviously not deleted
		Id = command.AggregateId;
		Title = command.Title;
		Body = command.Body;
		DueDate = command.DueDate;
	}

	internal void When(TodoUpdateCommand command)
	{
		ValidateBody(command.Body);
		ValidateTitle(command.Title);
		ValidateDueDate(command.DueDate);

		Title = command.Title;
		Body = command.Body;
		DueDate = command.DueDate;
	}

	internal void When(TodoUpdateDueDateCommand command)
	{
		ValidateDueDate(command.DueDate);

		DueDate = command.DueDate;
	}

	internal void When(TodoDeleteCommand command)
	{
		// Consider validating whether the todo is in a valid state to be deleted, there could be e.g. related aggregates depending on this
		Deleted = true;
	}

	// Validation rules are kept separately from setting the actual values to allow us to complete all validation for a given event, prior to setting any properties.
	// Often they can be kept static as well, but feel free to implement non-static methods where necessary.
	private static void ValidateBody(string body)
	{
		ArgumentNullException.ThrowIfNull(body);
	}

	private static void ValidateTitle(string title)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			throw new ArgumentException("Title cannot be empty", nameof(title));
		}
	}

	private static void ValidateDueDate(DateTime dueDate)
	{
		if (dueDate < DateTime.UtcNow)
		{
			throw new ArgumentOutOfRangeException(nameof(dueDate), "DueDate must be in the future");
		}
	}
}