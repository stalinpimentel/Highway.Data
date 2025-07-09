using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Highway.Data;

public abstract class SqlScalar<T> : IScalar<T>
{
    protected Func<SqlConnection, T> ContextQuery;

    public T Execute(IDataSource context)
    {
        if (context is not DbContext efContext)
        {
            throw new ArgumentException($"{nameof(context)} must be of type {nameof(DbContext)}.", nameof(context));
        }
        using var conn = new SqlConnection(efContext.Database.GetConnectionString());

        return ContextQuery.Invoke(conn);
    }
}