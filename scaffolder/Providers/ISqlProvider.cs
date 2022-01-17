using System;
using System.Collections.Generic;
using scaffolder.Types;

namespace scaffolder.Providers
{
    public interface ISqlProvider
    {
        string ConnectionString { get; set; }
        IEnumerable<string> GetAvailableTables();
        List<Table> GetFullTableInfo(IEnumerable<string> tableList);
        bool TestConnection();
    }
}
