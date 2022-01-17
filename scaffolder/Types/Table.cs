using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scaffolder.Types
{
    public class Table
    {
        public string AdoAdapterNamespace { get; set; }
        public string AdoAdapterConnectionClassName { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public List<Column> Columns { get; set; }
        public Column PrimaryKey { get
            {
                if (Columns == null)
                    return null;
                if (Columns.Count == 0)
                    return null;
                return Columns.Where(x => x.IsPrimaryKey).FirstOrDefault();
            }
        }
    }
}
