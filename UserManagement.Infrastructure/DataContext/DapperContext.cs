using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace UserManagement.Infrastructure.DataContext;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;

        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }
    // Establishing a connection to the database
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
    // Basic query method for selecting multiple records
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.QueryAsync<T>(sql, param);
        }
    }
    // Query a single record (first or default)
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }
    }
    public T QueryFirstOrDefault<T>(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return connection.QueryFirstOrDefault<T>(sql, param);
        }
    }
    // Query a single record (single value, returns scalar)
    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }
    }

    // Execute a command (for insert, update, delete) and return the number of affected rows
    public async Task<int> ExecuteAsync(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteAsync(sql, param);
        }
    }
    public int Execute(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return connection.Execute(sql, param);
        }
    }

    // Execute a command and return a single value
    public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteScalarAsync<T>(sql, param);
        }
    }
    public T ExecuteScalar<T>(string sql, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return connection.ExecuteScalar<T>(sql, param);
        }
    }


    // Query stored procedure and return multiple results
    public async Task<IEnumerable<T>> QueryStoredProcAsync<T>(string storedProcName, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.QueryAsync<T>(storedProcName, param, commandType: CommandType.StoredProcedure);
        }
    }

    // Query stored procedure and return a single result
    public async Task<T> QueryStoredProcFirstOrDefaultAsync<T>(string storedProcName, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<T>(storedProcName, param, commandType: CommandType.StoredProcedure);
        }
    }

    // Execute a stored procedure that does not return a result set
    public async Task<int> ExecuteStoredProcAsync(string storedProcName, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteAsync(storedProcName, param, commandType: CommandType.StoredProcedure);
        }
    }

    // Execute a stored procedure and return a scalar value
    public async Task<T> ExecuteStoredProcScalarAsync<T>(string storedProcName, object param = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteScalarAsync<T>(storedProcName, param, commandType: CommandType.StoredProcedure);
        }
    }
}
