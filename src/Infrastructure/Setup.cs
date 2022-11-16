using Dapper.DDD.Repository;
using Dapper.DDD.Repository.DependencyInjection;
using Dapper.DDD.Repository.Sql;
using Domain.Todos.Aggregates;
using Domain.Todos.ValueObjects;
using Infrastructure.Requirements;
using Infrastructure.Todos.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Setup
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
	{
		services.ConfigureDapperRepositoryDefaults(config =>
		{
			config.Schema = "dbo";
			config.ConnectionFactory = new SqlConnectionFactory(connectionString);
			config.DapperInjectionFactory = new DapperInjectionFactory();
			config.QueryGeneratorFactory = new SqlQueryGeneratorFactory();
			config.AddTypeConverter(TodoId.New().GetTypeConverter());
		});
		services.AddTableRepository<Todo, TodoId, ITodoRepository, TodoRepository>(config =>
		{
			config.TableName = "Todos";
			config.HasKey(e => e.Id);
			config.Ignore(e => e.Deleted);
		});
		services.AddTransient<ITodoRepository, TodoRepository>();
		return services;
	}
}