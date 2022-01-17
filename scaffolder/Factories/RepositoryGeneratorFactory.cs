using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Types;
using scaffolder.Generators;

namespace scaffolder.Factories
{
    public class RepositoryGeneratorFactory
    {
        public static IRepositoryGenerator GetGenerator(RepositoryGeneratorType type)
        {
            switch (type)
            {
                case RepositoryGeneratorType.SimpleData:
                    return new SimpleDataRepositoryGenerator();
                //    break;
                case RepositoryGeneratorType.Dapper:
                    return new DapperRepositoryGenerator();
                //    break;
                default:
                    throw new ArgumentException("The selected repository type was not recognized.");
                //    break;
            }
        }
    }
}
