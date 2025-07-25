﻿using System.Collections.Generic;
using System.Linq;

namespace Highway.Data.Contexts.TypeRepresentations;

internal class ObjectRepresentation
{
    public ObjectRepresentation()
    {
        Parents = new Dictionary<object, Accessor>();
    }

    internal object Entity { get; set; }

    internal Dictionary<object, Accessor?> Parents { get; set; }

    internal IEnumerable<ObjectRepresentation> RelatedEntities { get; set; }

    public List<ObjectRepresentation> GetObjectRepresentationsToPrune()
    {
        return AllRelated().Where(x => x.Orphaned()).ToList();
    }

    public bool IsType<T1>()
    {
        return Entity.GetType() is T1;
    }

    public bool Orphaned()
    {
        if (!Parents.Any())
        {
            return true;
        }

        return
            Parents.All(
                accessor =>
                    accessor.Value == null || accessor.Value.GetterFunc == null ||
                    accessor.Value.GetterFunc(accessor.Key, Entity) == null);
    }

    internal IEnumerable<ObjectRepresentation> AllRelated()
    {
        var evaluatedObjects = new List<ObjectRepresentation>();

        return AllRelated(evaluatedObjects);
    }

    internal IEnumerable<ObjectRepresentation> AllRelated(List<ObjectRepresentation> evaluatedObjects)
    {
        var items = RelatedEntities.ToList();
        foreach (var objectRepresentationBase in RelatedEntities)
        {
            if (evaluatedObjects.Contains(objectRepresentationBase))
            {
                continue;
            }

            evaluatedObjects.Add(objectRepresentationBase);
            items.AddRange(objectRepresentationBase.AllRelated(evaluatedObjects));
        }

        return items;
    }
}