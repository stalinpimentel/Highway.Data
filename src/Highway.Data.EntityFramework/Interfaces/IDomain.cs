using System.Collections.Generic;

using Highway.Data.EventManagement.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data;

public interface IDomain
{
    DbContextOptions Options { get; }

    IContextConfiguration Context { get; }

    List<IInterceptor> Events { get; }

    IMappingConfiguration Mappings { get; }
}