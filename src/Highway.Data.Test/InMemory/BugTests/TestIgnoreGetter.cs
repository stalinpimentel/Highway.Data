﻿using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests;

[TestClass]
public class TestIgnoreGetter
{
    [TestMethod]
    public void ShouldNotTryToMapAndThrowNullExceptionWhenAccessingGetterOnlyPropertyThatReferencesANullObject()
    {
        var context = new InMemoryDataContext();

        context.Add(new Parent());
        context.Commit();

        var models = new GetParents().Execute(context);

        models.Should().HaveCount(1);
    }

    public class Child
    {
        public string Name { get; set; }
    }

    public class Parent
    {
        public Child Child { get; set; }

        public string ChildName => Child.Name;
    }

    public class GetParents : Query<Parent>
    {
        public GetParents()
        {
            ContextQuery = c => c.AsQueryable<Parent>();
        }
    }
}