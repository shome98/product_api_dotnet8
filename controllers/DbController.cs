using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace product_api_dotnet8.Controllers
{
    [ApiController]
    [Route("api/db")]
    public class DbController : ControllerBase
    {
        private readonly string _connectionString;

        public DbController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnectionPostgres");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string postgres 'DefaultConnection' is missing or invalid.");
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                return Ok(new
                {
                    status = "success",
                    message = "Connection successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets a list of all user-defined tables in the PostgreSQL database.
        /// </summary>

        [HttpGet("tables")]
        public async Task<IActionResult> GetTables()
        {
            const string sql = @"
                SELECT tablename 
                FROM pg_tables 
                WHERE schemaname = 'public' 
                ORDER BY tablename;";

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                var tables = await connection.QueryAsync<string>(sql);

                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to retrieve table names",
                    error = ex.Message
                });
            }
        }
    }
}
