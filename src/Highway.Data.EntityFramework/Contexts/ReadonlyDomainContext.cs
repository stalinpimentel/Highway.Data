using Common.Logging;
using Common.Logging.Simple;

namespace Highway.Data;

/// <summary>
///     A Context that is constrained to a specified Domain
/// </summary>
/// <typeparam name="T">The Domain this context is specific for</typeparam>
public class ReadonlyDomainContext<T> : ReadonlyDataContext, IReadonlyDomainContext<T>
    where T : class, IDomain
{
    public ReadonlyDomainContext(T domain)
        : base(domain.Options, domain.Mappings, new NoOpLogger())
    {
    }

    public ReadonlyDomainContext(T domain, ILog logger)
        : base(domain.Options, domain.Mappings, logger)
    {
    }
}