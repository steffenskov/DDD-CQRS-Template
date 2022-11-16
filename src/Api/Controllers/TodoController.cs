using Api.Models;
using Domain.Todos.Commands;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
	private readonly IMediator _mediator;

	public TodoController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public async Task<ActionResult<TodoViewModel>> PostAsync(TodoInputModel createModel,
		CancellationToken cancellationToken)
	{
		var command = new TodoCreateCommand(TodoId.New(), createModel.Title, createModel.Body, createModel.DueDate);

		var result = await _mediator.Send(command, cancellationToken);

		return new ObjectResult(new TodoViewModel(result))
		{
			StatusCode = 201
		};
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<TodoViewModel>> PatchDueDateAsync(TodoId id, TodoInputModel updateModel,
		CancellationToken cancellationToken)
	{
		var command = new TodoUpdateCommand(id, updateModel.Title, updateModel.Body, updateModel.DueDate);

		var result = await _mediator.Send(command, cancellationToken);
		return NoContent(); // We could return the view model instead if we wanted, as result is actually our aggregate
	}

	[HttpPatch("{id}/DueDate")]
	public async Task<IActionResult> PatchDueDateAsync(TodoId id, TodoDueDateInputModel updateModel,
		CancellationToken cancellationToken)
	{
		var command = new TodoUpdateDueDateCommand(id, updateModel.DueDate);

		var result = await _mediator.Send(command, cancellationToken);
		return NoContent(); // We could return the view model instead if we wanted, as result is actually our aggregate
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAsync(TodoId id, CancellationToken cancellationToken)
	{
		var command = new TodoDeleteCommand(id);

		var result = await _mediator.Send(command, cancellationToken);
		return NoContent(); // We could return the view model instead if we wanted, as result is actually our aggregate
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<TodoViewModel>> GetAsync(TodoId id, CancellationToken cancellationToken)
	{
		var command = new TodoGetSingleQuery(id);

		var result = await _mediator.Send(command, cancellationToken);
		if (result != null)
		{
			return new TodoViewModel(result!);
		}

		return NotFound();
	}

	[HttpGet]
	public async Task<ActionResult<ICollection<TodoViewModel>>> GetAllAsync(CancellationToken cancellationToken)
	{
		var command = new TodoGetAllQuery();

		var result = await _mediator.Send(command, cancellationToken);
		return result
			.Select(todo => new TodoViewModel(todo))
			.ToList();
	}
}