using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Highway.Data;

public abstract class SqlCommand : ICommand
{
    protected Action<SqlConnection>? ContextQuery;

    public void Execute(IDataContext context)
    {
        if (ContextQuery is null)
        {
            return;
        }

        if (context is not DbContext efContext)
        {
            return;
        }
        
        if (!efContext.Database.IsRelational())
        {
            throw new InvalidOperationException("The database is not a relational database context.");
        }

        using var conn = new SqlConnection(efContext.Database.GetConnectionString());
        ContextQuery.Invoke(conn);
    }
}