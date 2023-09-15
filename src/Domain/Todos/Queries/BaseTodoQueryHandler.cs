namespace Domain;

internal abstract class BaseTodoQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
where TQuery : IRequest<TResult>
{
	protected readonly ITodoRepository _repository;

	protected BaseTodoQueryHandler(ITodoRepository repository)
	{
		this._repository = repository;
	}

	public abstract Task<TResult> Handle(TQuery request, CancellationToken cancellationToken);
}
