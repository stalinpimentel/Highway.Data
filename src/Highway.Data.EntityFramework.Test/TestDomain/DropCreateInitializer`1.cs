using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.EntityFramework.Test.TestDomain;

/// <summary>
///     ****    FOR DEVELOPMENT USAGE ONLY              ****
///     ****    DO NOT PUT THIS IN PRODUCTION CODE      ****
///     This class will clear the existing connections to the database and drop the database
/// </summary>
public class DropCreateInitializer<T>
    where T : DbContext
{
    private readonly Action<T>? _seedAction;
    private readonly Func<IEnumerable<string>>? _storedProcs;

    /// <summary>
    /// </summary>
    /// <param name="seedAction">actions to execute</param>
    /// <param name="storedProcs">stored procedure strings</param>
    public DropCreateInitializer(Action<T>? seedAction = null, Func<IEnumerable<string>>? storedProcs = null)
    {
        _seedAction = seedAction;
        _storedProcs = storedProcs;
    }

    public void InitializeDatabase(T context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        if (_storedProcs is not null)
        {
            foreach (var sp in _storedProcs())
            {
                if (string.IsNullOrWhiteSpace(sp))
                {
                    continue;
                }

                context.Database.ExecuteSqlRaw(sp);
            }
        }

        if (_seedAction is null)
        {
            _seedAction(context);
            context.SaveChanges();
        }
    }
}