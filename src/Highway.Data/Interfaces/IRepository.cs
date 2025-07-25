﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Highway.Data;

/// <summary>
///     The interface used to interact with the ORM Specific Implementations
/// </summary>
public interface IRepository
{
    /// <summary>
    ///     Reference to the Context the repository, allowing for create, update, and delete
    /// </summary>
    IUnitOfWork Context { get; }

    /// <summary>
    ///     Executes a prebuilt <see cref="ICommand" />
    /// </summary>
    /// <param name="command">The prebuilt command object</param>
    void Execute(ICommand command);

    /// <summary>
    ///     Executes a prebuilt <see cref="ICommand" /> asynchronously
    /// </summary>
    /// <param name="command">The prebuilt command object</param>
    /// <param name="cancellationToken"></param>
    Task ExecuteAsync(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
    /// </summary>
    /// <typeparam name="T">The Entity being queried</typeparam>
    /// <param name="query">The prebuilt Query Object</param>
    /// <returns>The <see cref="IEnumerable{T}" /> returned from the query</returns>
    IEnumerable<T> Find<T>(IQuery<T> query);

    /// <summary>
    ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
    /// </summary>
    /// <typeparam name="T">The Entity being queried</typeparam>
    /// <param name="query">The prebuilt Query Object</param>
    /// <returns>The instance of <typeparamref name="T" /> returned from the query</returns>
    T Find<T>(IScalar<T> query);

    /// <summary>
    ///     Executes a prebuilt <see cref="IScalar{T}" /> and returns a single instance of <typeparamref name="T" />
    /// </summary>
    /// <typeparam name="T">The Entity being queried</typeparam>
    /// <param name="query">The prebuilt Query Object</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The task that will return an instance of <typeparamref name="T" /> from the query</returns>
    Task<T> FindAsync<T>(IScalar<T> query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
    /// </summary>
    /// <typeparam name="T">The Entity being queried</typeparam>
    /// <param name="query">The prebuilt Query Object</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
    Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Executes a prebuilt <see cref="IQuery{T}" /> and returns an <see cref="IEnumerable{T}" />
    /// </summary>
    /// <typeparam name="TSelection">The Entity being queried from the data store.</typeparam>
    /// <typeparam name="TProjection">The type being returned to the caller.</typeparam>
    /// <param name="query">The prebuilt Query Object</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The task that will return <see cref="IEnumerable{T}" /> from the query</returns>
    Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query, CancellationToken cancellationToken = default)
        where TSelection : class;
}