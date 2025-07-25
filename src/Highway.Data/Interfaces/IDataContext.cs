﻿using System;

using Highway.Data.Interceptors.Events;

namespace Highway.Data;

/// <summary>
///     The standard interface used to interact with an ORM specific implementation
/// </summary>
public interface IDataContext : IDataSource, IDisposable, IUnitOfWork
{
    /// <summary>
    ///     The event fired just before the commit of the persistence
    /// </summary>
    event EventHandler<BeforeSaveEventArgs> BeforeSave;

    /// <summary>
    ///     The event fired just after the commit of the persistence
    /// </summary>
    event EventHandler<AfterSaveEventArgs> AfterSave;
}