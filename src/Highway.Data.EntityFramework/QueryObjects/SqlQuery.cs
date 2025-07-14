using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Highway.Data;

public class SqlQuery<T> : IQuery<T>
{
    protected Func<DbConnection, IEnumerable<T>> ContextQuery;

    public string SqlStatement { get; set; }

    public IEnumerable<T> Execute(IDataSource context)
    {
        if (context is not DbContext efContext)
        {
            throw new InvalidOperationException("You cannot execute EF Sql Queries against a non-EF context");
        }

        using var conn = new SqlConnection(efContext.Database.GetConnectionString());

        return ContextQuery.Invoke(conn);
    }

    public string OutputQuery(IDataSource context)
    {
        return SqlStatement;
    }

    public string OutputSQLStatement(IDataSource context)
    {
        return OutputQuery(context);
    }
}