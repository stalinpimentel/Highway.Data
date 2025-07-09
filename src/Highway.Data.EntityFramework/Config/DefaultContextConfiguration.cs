using Microsoft.EntityFrameworkCore;

namespace Highway.Data;

/// <summary>
///     Configuration for the context that takes the Highway.Data opinions.
/// </summary>
public class DefaultContextConfiguration : IContextConfiguration
{
    /// <summary>
    ///     Configures context without lazy loading or proxy creation
    /// </summary>
    /// <param name="context"></param>
    public void ConfigureContext(DbContext context)
    {
        // TODO: None of this is actually used in EF Core, but we keep it to remove later
        // context.Configuration.LazyLoadingEnabled = false;
        // context.Configuration.ProxyCreationEnabled = false;
    }
}