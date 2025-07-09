using System;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging.Simple;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Highway.Data.ReadonlyTests;

[TestClass]
public class ReadonlyDbContextTests
{
    private static ReadonlyDbContext _target;

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        var options = new DbContextOptionsBuilder()
            .UseSqlServer(Configuration.Instance.TestDatabaseConnectionString, opts => opts.EnableRetryOnFailure())
            .Options;
        _target = new ReadonlyDbContext(options, new NoOpLogger());
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void CannotCallGenericSet()
    {
        var students = _target.Set<Student>();
        students.Add(new Student());
        _target.SaveChanges();
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void CannotCallSaveChanges()
    {
        _target.SaveChanges();
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public async Task CannotCallSaveChangesAsync()
    {
        await _target.SaveChangesAsync();
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public async Task CannotCallSaveChangesAsyncWithCancellationToken()
    {
        await _target.SaveChangesAsync(CancellationToken.None);
    }
}