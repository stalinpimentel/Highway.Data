using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data;

/// <summary>
///     A Context that is constrained to a specified Domain
/// </summary>
/// <typeparam name="T">The Domain this context is specific for</typeparam>
public class DomainContext<T> : DataContext, IDomainContext<T>
    where T : class, IDomain
{
    /// <summary>
    ///     Constructs the domain context
    /// </summary>
    /// <param name="domain"></param>
    public DomainContext(T domain)
        : base(domain.Options, new NoOpLogger(), domain.Mappings)
    {
    }

    /// <summary>
    ///     Constructs the domain context
    /// </summary>
    /// <param name="domain">domain for context</param>
    /// <param name="logger">logger</param>
    public DomainContext(T domain, ILog logger)
        : base(domain.Options, logger, domain.Mappings)
    {
    }
}