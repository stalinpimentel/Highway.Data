using Common.Logging;
using Common.Logging.Simple;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.Factories;

/// <summary>
///     Simple factory for constructing repositories
/// </summary>
public class RepositoryFactory : IRepositoryFactory
{
    private readonly string _connectionString;
    private readonly IContextOptionsConfigurator? _contextConfig;
    private readonly ILog _logger;
    private readonly IMappingConfiguration? _mappings;

    /// <summary>
    ///     Creates a repository factory for the supplied list of domains
    /// </summary>
    public RepositoryFactory(string connectionString, IMappingConfiguration mappings)
        : this(connectionString, mappings, EmptyContextOptionsConfigurator.Instance, new NoOpLogger())
    {
    }

    /// <summary>
    ///     Creates a repository factory for the supplied list of domains
    /// </summary>
    public RepositoryFactory(string connectionString, IMappingConfiguration mappings, IContextOptionsConfigurator? contextConfiguration)
        : this(connectionString, mappings, contextConfiguration, new NoOpLogger())
    {
    }

    /// <summary>
    ///     Creates a repository factory for the supplied list of domains
    /// </summary>
    public RepositoryFactory(string connectionString, IMappingConfiguration mappings, ILog logger)
        : this(connectionString, mappings, EmptyContextOptionsConfigurator.Instance, logger)
    {
    }

    /// <summary>
    ///     Creates a repository factory for the supplied list of domains
    /// </summary>
    public RepositoryFactory(string connectionString, IMappingConfiguration mappings, IContextOptionsConfigurator? contextConfig, ILog logger)
    {
        _connectionString = connectionString;
        _mappings = mappings;
        _contextConfig = contextConfig;
        _logger = logger;
    }

    /// <summary>
    ///     Creates a repository for the requested domain
    /// </summary>
    /// <returns>Domain specific repository</returns>
    public IRepository Create()
    {
        var builder = new DbContextOptionsBuilder()
                      .UseSqlServer(_connectionString, opts => opts.EnableRetryOnFailure())
                      .LogTo(_logger.Info);
        
        var options = _contextConfig is not null
            ? _contextConfig.ConfigureContextOptions(builder).Options
            : builder.Options;
        
        return new Repository(new DataContext(options, _logger, _mappings));
    }
}