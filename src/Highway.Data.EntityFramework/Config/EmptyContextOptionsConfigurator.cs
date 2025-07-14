namespace Highway.Data;

/// <summary>
///     Configuration for the context that takes the Highway.Data opinions.
/// </summary>
public class EmptyContextOptionsConfigurator : IContextOptionsConfigurator
{
    public static readonly EmptyContextOptionsConfigurator Instance = new();

    protected EmptyContextOptionsConfigurator()
    {
    }
}