using System;
using Microsoft.EntityFrameworkCore;

namespace product_api_dotnet8.Db;

public class DataContextEFPostgres : DbContext
{
    public DataContextEFPostgres(DbContextOptions<DataContextEFPostgres> options)
            : base(options)
    {

    }

    public DbSet<Item> Items { get; set; }

}
