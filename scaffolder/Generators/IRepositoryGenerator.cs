using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Types;

namespace scaffolder.Generators
{
    public interface IRepositoryGenerator
    {
        string GetUsings(Table tableInfo);
        string GetRepositoryMethods(Table tableInfo);
    }
}
