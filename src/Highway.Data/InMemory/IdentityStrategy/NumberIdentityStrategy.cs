using System;
using System.Linq.Expressions;
using System.Numerics;

namespace Highway.Data.Contexts;

public class NumberIdentityStrategy<TType, TIdentity> : IdentityStrategy<TType, TIdentity>
    where TType : class
    where TIdentity : INumber<TIdentity>
{
    protected NumberIdentityStrategy(Expression<Func<TType, TIdentity>> property)
        : base(property)
    {
        Generator = GenerateValue;
    }

    protected override bool IsDefaultUnsetValue(TIdentity? id) => id == TIdentity.Zero;

    private TIdentity GenerateValue()
    {
        SetLastValue(LastValue + TIdentity.One);
        return LastValue;
    }
}
