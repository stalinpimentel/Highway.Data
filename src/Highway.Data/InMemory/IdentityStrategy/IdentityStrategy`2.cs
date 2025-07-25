﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace Highway.Data.Contexts;

/// <summary>
///     A base implementation of IIdentityStrategy.
/// </summary>
/// <typeparam name="TType">The type of the entities that will have identity values assigned.</typeparam>
/// <typeparam name="TIdentity">The type of the identity values to be assigned.</typeparam>
public abstract class IdentityStrategy<TType, TIdentity> : IIdentityStrategy<TType>
    where TType : class
{
    private readonly Action<TType> _identitySetter;

    private readonly Lock _lastValueLock = new();

    /// <summary>
    ///     Creates an instance of <see cref="IdentityStrategy{TType,TIdentity}" /> using the provided identity
    ///     <paramref name="property" /> setter.
    /// </summary>
    /// <param name="property">The property setter used to set the identity value of an entity.</param>
    protected IdentityStrategy(Expression<Func<TType, TIdentity>> property)
    {
        _identitySetter = obj =>
        {
            var propertyInfo = GetPropertyFromExpression(property);
            var id = (TIdentity)propertyInfo.GetValue(obj, null);
            if (IsDefaultUnsetValue(id))
            {
                propertyInfo.SetValue(obj, Next(), null);
            }
        };
    }

    /// <summary>
    ///     The function used to generate identity values.
    /// </summary>
    public Func<TIdentity>? Generator { get; protected set; } = null;

    /// <summary>
    ///     The last value used to set an identity value.
    /// </summary>
    public TIdentity LastValue { get; protected set; }

    /// <summary>
    ///     Assigns an identity value to the given <paramref name="entity" />.
    /// </summary>
    /// <param name="entity"></param>
    public void Assign(TType entity)
    {
        _identitySetter.Invoke(entity);
    }

    /// <summary>
    ///     Invokes the generator to set the next appropriate value for the identity value.
    /// </summary>
    /// <returns>The next appropriate value for the identity value.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public TIdentity Next()
    {
        if (Generator is null)
        {
            throw new NotSupportedException("A generator function has not been set for this identity strategy.");
        }

        return Generator.Invoke();
    }

    /// <summary>
    ///     Returns a value indicating whether a given value equals the default, unset identity value.
    /// </summary>
    /// <param name="id">The identity value to examine.</param>
    /// <returns>A value indicating whether a given value equals the default, unset identity value.</returns>
    protected abstract bool IsDefaultUnsetValue(TIdentity id);

    /// <summary>
    ///     A thread-safe method for setting the LastValue property
    /// </summary>
    /// <param name="value"></param>
    protected void SetLastValue(TIdentity value)
    {
        lock (_lastValueLock)
        {
            LastValue = value;
        }
    }

    private static PropertyInfo GetPropertyFromExpression(Expression<Func<TType, TIdentity>> lambda)
    {
        MemberExpression memberExpression;

        // this line is necessary, because sometimes the expression 
        // comes as Convert(originalExpression)
        if (lambda.Body is UnaryExpression bodyExpression)
        {
            if (bodyExpression.Operand is MemberExpression operand)
            {
                memberExpression = operand;
            }
            else
            {
                throw new ArgumentException();
            }
        }
        else if (lambda.Body is MemberExpression body)
        {
            memberExpression = body;
        }
        else
        {
            throw new ArgumentException("The expression must be a unary or member expression.", nameof(lambda));
        }

        return (PropertyInfo)memberExpression.Member;
    }
}