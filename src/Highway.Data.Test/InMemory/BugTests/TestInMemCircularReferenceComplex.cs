﻿using System.Linq;

using FluentAssertions;

using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory.BugTests;

[TestClass]
public class TestInMemCircularReferenceComplex
{
    private Child _child;

    private IDataContext _context;

    private GrandParent _grandParent;

    private Parent _parent;

    [TestInitialize]
    public void SetUp()
    {
        _context = new InMemoryDataContext();

        _grandParent = new GrandParent();
        _parent = new Parent();
        _child = new Child();

        _grandParent.Child = _parent;
        _grandParent.GrandChild = _child;

        _parent.Child = _child;
        _parent.DirectParent = _grandParent;

        _child.Parent = _parent;
        _child.GrandParent = _grandParent;

        _context.Add(_parent);
        _context.Commit();
    }

    [TestMethod]
    public void ShouldAllowCircularHierarchies()
    {
        var grandparent = new CircularReference
            { Id = 1, Outer = null };

        var parent = new CircularReference
            { Id = 2, Outer = grandparent };

        var child = new CircularReference
            { Id = 3, Outer = parent };

        grandparent.Inner = parent;
        parent.Inner = child;

        _context.Add(grandparent);
        _context.Commit();

        var circularReferences = _context.AsQueryable<CircularReference>();
        circularReferences.Should().HaveCount(3);
        circularReferences.Single(x => x.Id == 1).Inner.Should().BeSameAs(parent);
        circularReferences.Single(x => x.Id == 2).Inner.Should().BeSameAs(child);
        circularReferences.Single(x => x.Id == 3).Inner.Should().BeNull();
        circularReferences.Single(x => x.Id == 3).Outer.Should().BeSameAs(parent);
        circularReferences.Single(x => x.Id == 2).Outer.Should().BeSameAs(grandparent);
        circularReferences.Single(x => x.Id == 1).Outer.Should().BeNull();
    }

    [TestMethod]
    public void ShouldGetSingleChild()
    {
        var child = _context.AsQueryable<Child>().Single();

        AssertEntities(child.GrandParent, child.Parent, child);
    }

    [TestMethod]
    public void ShouldGetSingleGrandParent()
    {
        var grandParent = _context.AsQueryable<GrandParent>().Single();

        AssertEntities(grandParent, grandParent.Child, grandParent.GrandChild);
    }

    [TestMethod]
    public void ShouldGetSingleParent()
    {
        var parent = _context.AsQueryable<Parent>().Single();

        AssertEntities(parent.DirectParent, parent, parent.Child);
    }

    private void AssertEntities(GrandParent grandParent, Parent parent, Child child)
    {
        _grandParent.Should().Be(grandParent);
        _parent.Should().Be(parent);
        _child.Should().Be(child);
    }

    private class CircularReference
    {
        public int Id { get; set; }

        public CircularReference Inner { get; set; }

        public CircularReference Outer { get; set; }
    }

    private class Child
    {
        public GrandParent GrandParent { get; set; }

        public Parent Parent { get; set; }
    }

    private class Parent
    {
        public Child Child { get; set; }

        public GrandParent DirectParent { get; set; }
    }

    private class GrandParent
    {
        public Parent Child { get; set; }

        public Child GrandChild { get; set; }
    }
}