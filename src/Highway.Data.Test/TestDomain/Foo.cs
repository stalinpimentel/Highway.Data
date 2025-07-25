﻿using System.Collections.Generic;

namespace Highway.Data.Tests.TestDomain;

public class Foo : IIdentifiable<int>
{
    public virtual string Address { get; set; }

    public Bar Bar { get; set; }

    public ICollection<Bar> Bars { get; set; }

    public virtual int Id { get; set; }

    public virtual string Name { get; set; }
}