using System.Data;
using Dapper.DDD.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Requirements;

internal class SqlConnectionFactory : IConnectionFactory
{
	private readonly string _connectionString;

	public SqlConnectionFactory(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new ArgumentException("ConnectionString cannot be null or whitespace.", nameof(connectionString));
		}

		_connectionString = connectionString;
	}

	public IDbConnection CreateConnection()
	{
		return new SqlConnection(_connectionString);
	}
}