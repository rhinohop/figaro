using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Providers;
using scaffolder.Types;

namespace scaffolder.Factories
{
    public class SqlProviderFactory
    {
        public static ISqlProvider GetSqlProvider(SqlProviderType type)
        {
            switch (type)
            {
                case SqlProviderType.SqlServer:
                    return new MsSqlProvider();
                //    break;
                case SqlProviderType.PostgreSql:
                    return new PgSqlProvider();
                //    break;
                default:
                    throw new ArgumentException("The selected sql provider type was not recognized.");
                //    break;
            }
        }
    }
}
