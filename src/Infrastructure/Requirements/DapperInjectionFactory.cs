using Dapper.DDD.Repository.Interfaces;

namespace Infrastructure.Requirements;

internal class DapperInjectionFactory : IDapperInjectionFactory
{
	public IDapperInjection<T> Create<T>()
	{
		return new DapperInjection<T>();
	}
}