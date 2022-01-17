using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Providers;
using scaffolder.Types;

namespace scaffolder.Types
{
    public class ScaffoldingParameters
    {
        public ISqlProvider SelectedProvider { get; set; }
        public List<Table> TablesToScaffold { get; set; }
        public String Database { get; set; }
    }
}
