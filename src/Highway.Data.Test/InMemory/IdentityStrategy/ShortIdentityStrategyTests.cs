﻿using Highway.Data.Contexts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.Tests.InMemory;

[TestClass]
public class ShortIdentityStrategyTests
{
    private ShortIdentityStrategy<Entity> _target;

    [TestMethod]
    public void Assign_ShouldAssignId()
    {
        // Arrange
        var entity = new Entity { Id = 0 };

        // Act
        _target.Assign(entity);

        // Assert
        Assert.AreEqual(1, entity.Id);
    }

    [TestMethod]
    public void Next_ShouldReturnNextValue()
    {
        // Arrange

        // Act
        var result = _target.Next();

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestInitialize]
    public void Setup()
    {
        _target = new ShortIdentityStrategy<Entity>(x => x.Id);
    }

    private class Entity
    {
        public short Id { get; set; }
    }
}