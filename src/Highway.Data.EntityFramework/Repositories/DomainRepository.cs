﻿using Highway.Data.EventManagement;

namespace Highway.Data.Repositories;

public class DomainRepository<T> : Repository, IDomainRepository<T>
    where T : class, IDomain
{
    private readonly EventManager<T> _eventManager;

    public DomainRepository(IDomainContext<T> context, T domain)
        : base(context)
    {
        _eventManager = new EventManager<T>(this);

        if (domain.Events is null)
        {
            return;
        }

        foreach (var @event in domain.Events)
        {
            _eventManager.Register(@event);
        }
    }

    public IDomainContext<T> DomainContext => (IDomainContext<T>)Context;
}