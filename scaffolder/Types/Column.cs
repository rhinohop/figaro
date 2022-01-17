using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scaffolder.Types
{
    public class Column
    {
        public Int32 ColumnOrder { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool Nullable { get; set; }
        public Int32 KeyType { get; set; }
        public bool IsPrimaryKey { get { return KeyType == 1; } }
        public bool Identity { get; set; }
        public bool IsForeign { get { return KeyType == 2; } }
        public String ForeignTable { get; set; }
    }
}
