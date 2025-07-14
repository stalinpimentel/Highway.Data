using Microsoft.EntityFrameworkCore;

namespace Highway.Data;

/// <summary>
///     Implement this interface to pass the context specific mapping to the constructor
/// </summary>
public interface IContextOptionsConfigurator
{
    /// <summary>
    ///     This method allows the configuration of context specific properties to be injected
    /// </summary>
    /// <param name="optionsBuilder">Instance of the options with which the context is going to be created.</param>
    DbContextOptionsBuilder ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder) => optionsBuilder;
}