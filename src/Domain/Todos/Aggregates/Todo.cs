using Domain.Todos.Commands;
using Domain.Todos.ValueObjects;

namespace Domain.Todos.Aggregates;

// The aggregate class is public, but all command handling is kept internal to force the developer to go through CQRS.
public record Todo : IAggregate<TodoId>
{
	public string Title { get; private init; } = default!;
	public string Body { get; private init; } = default!;
	public DateTime DueDate { get; private init; }
	public bool Deleted { get; private init; }
	public TodoId Id { get; private init; } = default!;

	internal Todo With(TodoCreateCommand command)
	{
		// Validate ALL domain rules relevant for this event, prior to setting any properties to avoid an aggregate stuck in limbo.
		ValidateBody(command.Body);
		ValidateTitle(command.Title);
		ValidateDueDate(command.DueDate);

		return this with
		{
			Deleted = false, // A new todo is obviously not deleted
			Id = command.AggregateId,
			Title = command.Title,
			Body = command.Body,
			DueDate = command.DueDate
		};
	}

	// Command handling returns the modified aggregate, this is because immutable aggregates are inherently thread-safe and as such safe to cache too.
	// If this is not a concern in your application, you can just use ordinary classes and modify the state of this instead.
	internal Todo With(TodoUpdateCommand command)
	{
		ValidateBody(command.Body);
		ValidateTitle(command.Title);
		ValidateDueDate(command.DueDate);

		return this with
		{
			Title = command.Title,
			Body = command.Body,
			DueDate = command.DueDate
		};
	}

	internal Todo With(TodoUpdateDueDateCommand command)
	{
		ValidateDueDate(command.DueDate);

		return this with
		{
			DueDate = command.DueDate
		};
	}

	internal Todo With(TodoDeleteCommand command)
	{
		// Consider validating whether the todo is in a valid state to be deleted, there could be e.g. related aggregates depending on this
		return this with
		{
			Deleted = true
		};
	}

	// Validation rules are kept separately from setting the actual values to allow us to complete all validation for a given command, prior to setting any properties.
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