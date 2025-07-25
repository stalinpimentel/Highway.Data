﻿using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Highway.Data.ReadonlyTests;

public class GetStudentByName : Scalar<Student>
{
    public GetStudentByName(string name)
    {
        ContextQuery = source => source.AsQueryable<Student>()
                                       .Include(x => x.Grade)
                                       .Single(x => x.Name == name);
    }
}