using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using Common.Logging;
using Common.Logging.Simple;

using Highway.Data.EntityFramework;
using Highway.Data.Interceptors.Events;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Highway.Data;

/// <summary>
///     A base implementation of the Code First Data DataContext for Entity Framework
/// </summary>
public class DataContext : DbContext, IEntityDataContext
{
    private readonly bool _databaseFirst;
    private readonly ILog _log;
    private readonly IMappingConfiguration? _mapping;
    
    /// <summary>
    ///     Database first way to construct the data context for Highway.Data.EntityFramework
    /// </summary>
    /// <param name="options">
    ///     The metadata embedded connection string from database first Entity
    ///     Framework
    /// </param>
    public DataContext(DbContextOptions options)
        : this(options, new NoOpLogger())
    {
    }

    /// <summary>
    ///     Database first way to construct the data context for Highway.Data.EntityFramework
    /// </summary>
    /// <param name="options">
    ///     The metadata embedded connection string from database first Entity
    ///     Framework
    /// </param>
    /// <param name="log">The logger for the database first context</param>
    public DataContext(DbContextOptions options, ILog log)
        : base(options)
    {
        _databaseFirst = true;
        _log = log;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="options">The db connection.</param>
    /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
    public DataContext(DbContextOptions options, IMappingConfiguration mapping)
        : this(options, new NoOpLogger(), mapping, new DefaultContextConfiguration())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="options">The db connection.</param>
    /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
    /// <param name="log">The logger being supplied for this context ( Optional )</param>
    public DataContext(
        DbContextOptions options,
        IMappingConfiguration mapping,
        ILog log)
        : this(options, log, mapping, new DefaultContextConfiguration())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="options">The context options.</param>
    /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
    /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
    public DataContext(
        DbContextOptions<DataContext> options,
        IMappingConfiguration mapping,
        IContextConfiguration contextConfiguration)
        : this(options, new NoOpLogger(), mapping, contextConfiguration)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="options">The context option.</param>
    /// <param name="mapping">The Mapping Configuration that will determine how the tables and objects interact</param>
    /// <param name="contextConfiguration">The context specific configuration that will change context level behavior</param>
    /// <param name="log">The logger being supplied for this context ( Optional )</param>
    public DataContext(DbContextOptions options, ILog log, IMappingConfiguration? mapping, IContextConfiguration? contextConfiguration = null)
        : base(options)
    {
        _mapping = mapping;
        _log = log;
        contextConfiguration?.ConfigureContext(this);
    }

    public event EventHandler<BeforeSaveEventArgs>? BeforeSave;

    public event EventHandler<AfterSaveEventArgs>? AfterSave;

    /// <summary>
    ///     Adds the provided instance of <typeparamref name="T" /> to the data context
    /// </summary>
    /// <typeparam name="T">The Entity Type being added</typeparam>
    /// <param name="item">The <typeparamref name="T" /> you want to add</param>
    /// <returns>The <typeparamref name="T" /> you added</returns>
    public new virtual T Add<T>(T item)
        where T : class
    {
        _log.DebugFormat("Adding Object {0}", item);
        Set<T>().Add(item);
        _log.TraceFormat("Added Object {0}", item);

        return item;
    }

    /// <summary>
    ///     This gives a mockable wrapper around the normal <see cref="DbSet{T}" /> method that allows for testability
    ///     This method is now virtual to allow for the injection of cross cutting concerns.
    /// </summary>
    /// <typeparam name="T">The Entity being queried</typeparam>
    /// <returns>
    ///     <see cref="IQueryable{T}" />
    /// </returns>
    public virtual IQueryable<T> AsQueryable<T>()
        where T : class
    {
        _log.DebugFormat("Querying Object {0}", typeof(T).Name);
        var result = Set<T>();
        _log.TraceFormat("Queried Object {0}", typeof(T).Name);

        return result;
    }

    /// <summary>
    ///     Attaches the provided instance of <typeparamref name="T" /> to the data context
    /// </summary>
    /// <typeparam name="T">The Entity Type being attached</typeparam>
    /// <param name="item">The <typeparamref name="T" /> you want to attach</param>
    /// <returns>The <typeparamref name="T" /> you attached</returns>
    public new virtual T Attach<T>(T item)
        where T : class
    {
        _log.DebugFormat("Attaching Object {0}", item);
        Set<T>().Attach(item);
        _log.TraceFormat("Attached Object {0}", item);

        return item;
    }

    /// <summary>
    ///     Commits all currently tracked entity changes
    /// </summary>
    /// <returns>the number of rows affected</returns>
    public virtual int Commit()
    {
        OnBeforeSave();
        _log.Trace("\tCommit");
        ChangeTracker.DetectChanges();
        var result = SaveChanges();
        _log.DebugFormat("\tCommitted {0} Changes", result);
        OnAfterSave();

        return result;
    }

    /// <summary>
    ///     Commits all currently tracked entity changes asynchronously
    /// </summary>
    /// <returns>the number of rows affected</returns>
    public virtual async Task<int> CommitAsync()
    {
        OnBeforeSave();
        _log.Trace("\tCommit");
        ChangeTracker.DetectChanges();
        var result = await SaveChangesAsync();
        _log.DebugFormat("\tCommitted {0} Changes", result);
        OnAfterSave();

        return result;
    }

    /// <summary>
    ///     Detaches the provided instance of <typeparamref name="T" /> from the data context
    /// </summary>
    /// <typeparam name="T">The Entity Type being detached</typeparam>
    /// <param name="item">The <typeparamref name="T" /> you want to detach</param>
    /// <returns>The <typeparamref name="T" /> you detached</returns>
    public virtual T Detach<T>(T item)
        where T : class
    {
        _log.TraceFormat("Retrieving State Entry For Object {0}", item);
        var entry = GetChangeTrackingEntry(item);
        _log.DebugFormat("Detaching Object {0}", item);
        if (entry == null)
        {
            throw new InvalidOperationException("Cannot detach an object that is not attached to the current context.");
        }

        entry.State = EntityState.Detached;
        _log.TraceFormat("Detached Object {0}", item);

        return item;
    }

    /// <summary>
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="dbParams"></param>
    /// <returns></returns>
    public virtual int ExecuteFunction(string procedureName, params SqlParameter[] dbParams)
    {
        var parameters =
            dbParams.Select(x => $"{x.ParameterName} : {x.Value} : {x.TypeName}\t").ToArray();

        _log.TraceFormat("Executing Procedure {0}, with parameters {1}", procedureName, string.Join(",", parameters));

        return Database.SqlQueryRaw<int>(procedureName, dbParams).FirstOrDefault();
    }

    /// <summary>
    ///     Executes a SQL command and returns the standard int return from the query
    /// </summary>
    /// <param name="sql">The Sql Statement</param>
    /// <param name="dbParams">A List of Database Parameters for the Query</param>
    /// <returns>The rows affected</returns>
    public virtual int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
    {
        var parameters =
            dbParams.Select(x => $"{x.ParameterName} : {x.Value} : {x.DbType}\t").ToArray();

        _log.TraceFormat("Executing SQL {0}, with parameters {1}", sql, string.Join(",", parameters));

        return Database.ExecuteSqlRaw(sql, dbParams);
    }

    /// <summary>
    ///     Executes a SQL command and tries to map the returned dataset into an <see cref="IEnumerable{T}" />
    ///     The results should have the same column names as the Entity Type has properties
    /// </summary>
    /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
    /// <param name="sql">The Sql Statement</param>
    /// <param name="dbParams">A List of Database Parameters for the Query</param>
    /// <returns>An <see cref="IEnumerable{T}" /> from the query return</returns>
    public virtual IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
    {
        var parameters =
            dbParams.Select(x => $"{x.ParameterName} : {x.Value} : {x.DbType}\t").ToArray();

        _log.TraceFormat("Executing SQL {0}, with parameters {1}", sql, string.Join(",", parameters));

        return Database.SqlQueryRaw<T>(sql, dbParams);
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
        _log.TraceFormat("Retrieving State Entry For Object {0}", item);
        var entry = GetChangeTrackingEntry(item);
        _log.DebugFormat("Reloading Object {0}", item);
        if (entry is null)
        {
            throw new InvalidOperationException("You cannot reload an object that is not in the current Entity Framework data context");
        }

        entry.Reload();
        _log.TraceFormat("Reloaded Object {0}", item);

        return item;
    }

    /// <summary>
    ///     Removes the provided instance of <typeparamref name="T" /> from the data context
    /// </summary>
    /// <typeparam name="T">The Entity Type being removed</typeparam>
    /// <param name="item">The <typeparamref name="T" /> you want to remove</param>
    /// <returns>The <typeparamref name="T" /> you removed</returns>
    public new virtual T Remove<T>(T item)
        where T : class
    {
        _log.DebugFormat("Removing Object {0}", item);
        Set<T>().Remove(item);
        _log.TraceFormat("Removed Object {0}", item);

        return item;
    }

    /// <summary>
    ///     Updates the provided instance of <typeparamref name="T" /> in the data context
    /// </summary>
    /// <typeparam name="T">The Entity Type being updated</typeparam>
    /// <param name="item">The <typeparamref name="T" /> you want to update</param>
    /// <returns>The <typeparamref name="T" /> you updated</returns>
    public new virtual T Update<T>(T item)
        where T : class
    {
        _log.TraceFormat("Retrieving State Entry For Object {0}", item);
        var entry = GetChangeTrackingEntry(item);
        _log.DebugFormat("Updating Object {0}", item);
        if (entry is null)
        {
            throw new InvalidOperationException("Cannot Update an object that is not attached to the current Entity Framework data context");
        }

        entry.State = EntityState.Modified;
        _log.TraceFormat("Updated Object {0}", item);

        return item;
    }

    protected virtual EntityEntry<T> GetChangeTrackingEntry<T>(T item)
        where T : class
    {
        return Entry(item);
    }

    private void OnAfterSave()
    {
        AfterSave?.Invoke(this, AfterSaveEventArgs.None);
    }

    private void OnBeforeSave()
    {
        BeforeSave?.Invoke(this, BeforeSaveEventArgs.None);
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
            // TODO: Replace with an actual exception type that indicates this is not a code-first context
            throw new Exception("UnintentionalCodeFirstException");
        }

        _log.Debug("\tOnModelCreating");
        if (_mapping is not null)
        {
            _log.TraceFormat("\t\tMapping : {0}", _mapping.GetType().Name);
            _mapping.ConfigureModelBuilder(modelBuilder);
        }

        base.OnModelCreating(modelBuilder);
    }
}