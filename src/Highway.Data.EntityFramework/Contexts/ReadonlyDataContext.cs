using System;
using System.Linq;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Highway.Data;

public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyEntityDataContext
{
    /// <summary>
    ///     Constructs a readonly context
    /// </summary>
    /// <param name="options">The standard SQL connection string for the Database</param>
    /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
    /// <param name="contextConfig"></param>
    /// <param name="log"></param>
    public ReadonlyDataContext(DbContextOptions options, IMappingConfiguration? mapping, IContextConfiguration? contextConfig, ILog log)
        : base(options, mapping, contextConfig, log)
    {
    }

    /// <summary>
    ///     This gives a mockable wrapper around the normal <see cref="DbSet{TEntity}" /> method that allows for testability
    ///     This method is now virtual to allow for the injection of cross cutting concerns.
    /// </summary>
    /// <typeparam name="T">The Entity being queried</typeparam>
    /// <returns>
    ///     <see cref="IQueryable{T}" />
    /// </returns>
    public virtual IQueryable<T> AsQueryable<T>()
        where T : class
    {
        return Set<T>().AsQueryable();
    }

    /// <summary>
    ///     Reloads the provided instance of <typeparamref name="T" /> from the database
    /// </summary>
    /// <typeparam name="T">The Entity Type being reloaded</typeparam>
    /// <param name="item">The <typeparamref name="T" /> you want to reload</param>
    /// <returns>The <typeparamref name="T" /> you reloaded</returns>
    public virtual T Reload<T>(T item)
        where T : class
    {
        var entry = Entry(item);
        if (entry == null)
        {
            throw new InvalidOperationException("You cannot reload an object that is not in the current Entity Framework data context");
        }

        entry.Reload();

        return item;
    }
}