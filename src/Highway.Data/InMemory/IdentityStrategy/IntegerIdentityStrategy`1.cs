using System;
using System.Linq.Expressions;

namespace Highway.Data.Contexts;

/// <summary>
///     An implementation of <see cref="IdentityStrategy{TType,TIdentity}" /> for entities where the identity property has
///     type int
/// </summary>
/// <typeparam name="T">The type of the entities that will have identity values assigned.</typeparam>
public class IntegerIdentityStrategy<T> : NumberIdentityStrategy<T, int>
    where T : class
{
    /// <summary>
    ///     Creates an instance of <see cref="IdentityStrategy{TType,TIdentity}" /> for entities where the identity property
    ///     has type int.  Uses the provided identity <paramref name="property" /> setter.
    /// </summary>
    /// <param name="property">The property setter used to set the identity value of an entity.</param>
    public IntegerIdentityStrategy(Expression<Func<T, int>> property)
        : base(property)
    {
    }
}