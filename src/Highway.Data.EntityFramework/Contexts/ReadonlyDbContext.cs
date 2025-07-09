using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Highway.Data;

public class ReadonlyDbContext : DbContext
{
    private readonly bool _databaseFirst;
    private readonly ILog _log;
    private readonly IMappingConfiguration? _mapping;

    public ReadonlyDbContext(DbContextOptions options, IMappingConfiguration? mapping, IContextConfiguration? contextConfiguration, ILog log)
        : base(options)
    {
        _log = log;
        _mapping = mapping;
        contextConfiguration?.ConfigureContext(this);
    }

    public ReadonlyDbContext(DbContextOptions options, ILog log)
        : base(options)
    {
        _databaseFirst = true;
        _log = log;
    }
    
    public sealed override int SaveChanges()
    {
        throw new NotImplementedException($"Do not call {nameof(SaveChanges)} on a {nameof(ReadonlyDbContext)}.");
    }
    
    public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException($"Do not call {nameof(SaveChangesAsync)} on a {nameof(ReadonlyDbContext)}.");
    }

    public sealed override DbSet<TEntity> Set<TEntity>()
    {
        throw new NotImplementedException($"Do not call {nameof(Set)} on a {nameof(ReadonlyDbContext)}.");
    }

    public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
    {
        var parameters = dbParams.Select(x => $"{x.ParameterName} : {x.Value} : {x.DbType}\t").ToArray();

        _log.Trace($"Executing SQL {sql}, with parameters {string.Join(",", parameters)}");

        return Database.SqlQueryRaw<T>(sql, dbParams);
    }

    internal DbSet<TEntity> InnerSet<TEntity>()
        where TEntity : class
    {
        _log.Debug($"Querying Object {typeof(TEntity).Name}");
        var result = base.Set<TEntity>();
        _log.Trace($"Queried Object {typeof(TEntity).Name}");

        return result;
    }

    /// <summary>
    ///     This method is called when the model for a derived context has been initialized, but
    ///     before the model has been locked down and used to initialize the context.  The default
    ///     implementation of this method takes the <see cref="IMappingConfiguration" /> array passed in on construction and
    ///     applies them.
    ///     If no configuration mappings were passed it it does nothing.
    /// </summary>
    /// <remarks>
    ///     Typically, this method is called only once when the first instance of a derived context
    ///     is created.  The model for that context is then cached and is for all further instances of
    ///     the context in the app domain.  This caching can be disabled by setting the ModelCaching
    ///     property on the given ModelBuilder, but note that this can seriously degrade performance.
    ///     More control over caching is provided through use of the DbModelBuilder and DbContextFactory
    ///     classes directly.
    /// </remarks>
    /// <param name="modelBuilder">The builder that defines the model for the context being created</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_databaseFirst)
        {
            // TODO: Replace with a more appropriate exception type
            throw new Exception("UnintentionalCodeFirstException");
        }

        _log.Debug("\tOnModelCreating");
        if (_mapping is not null)
        {
            _log.Trace($"\t\tMapping : {_mapping.GetType().Name}");
            _mapping.ConfigureModelBuilder(modelBuilder);
        }

        base.OnModelCreating(modelBuilder);
    }

    protected bool ShouldValidateEntity(EntityEntry entityEntry)
    {
        throw new NotImplementedException($"Do not call {nameof(ShouldValidateEntity)} on a {nameof(ReadonlyDbContext)}.");
    }
}