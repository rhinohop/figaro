using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace scaffolder.Types
{
    public class Configuration
    {
        public string Namespace { get; set; }
        public string OutputPath { get; set; }
        public string TemplatePath { get; set; }
        public Int32 ApplicationPort { get; set; }
        public String ConnectionString { get; set; }
        public Boolean UseLogicDelete { get; set; }
        public String DeleteField { get; set; }

        public string ModelNamespace { get { return Namespace + ".Models"; } }
        public string ControllerNamespace { get { return Namespace + ".Controllers"; } }

        public bool GenerateClasses { get; set; }
        public bool GenerateControllers { get; set; }
        public bool IncludeDataAnnotations { get; set; }
        public bool IncludeRepositoryMethods { get; set; }

        public string ProjectOutputPath { get { return Path.Combine(OutputPath, Namespace); } }
        public string ClassOutputPath { get { return Path.Combine( ProjectOutputPath, "Models"); } }
        public string HelpersOutputPath { get { return Path.Combine(ProjectOutputPath, "Helpers"); } }
        public string ControllerOutputPath { get { return Path.Combine(ProjectOutputPath, "Controllers"); } }
        public string PropertiesOutputPath { get { return Path.Combine(ProjectOutputPath, "Properties"); } }

    }
}
