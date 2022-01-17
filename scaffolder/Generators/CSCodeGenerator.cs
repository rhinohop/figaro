using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Helpers;
using scaffolder.Types;

namespace scaffolder.Generators
{
    public class CSCodeGenerator
    {
        public CSCodeGenerator() { }

        public void Generate(List<Table> tables, Configuration config, String dbName)
        {
            StringBuilder databaseContext = new StringBuilder();
            databaseContext.AppendLine("using Microsoft.EntityFrameworkCore;");
            databaseContext.AppendLine();
            databaseContext.AppendFormat("namespace {0}.Models\n", config.Namespace);
            databaseContext.AppendLine("{");
            databaseContext.AppendFormat("\tpublic class {0}DB : DbContext\n", dbName.Replace(".", "_"));
            databaseContext.AppendLine("\t{");
            databaseContext.AppendFormat("\t\tpublic {0}DB(DbContextOptions<{0}DB> options) : base(options) {{ }}\n", dbName.Replace(".", "_"));
            databaseContext.AppendLine("\t\t");
            if (config.GenerateClasses)
            {
                var entityGenerator = new EntityGenerator(config);
                tables.ForEach(m => {
                    databaseContext.AppendFormat("public DbSet<{0}> {0} {{ get; set; }}\n", m.Name);
                    entityGenerator.Generate(m);
                });
            }
            databaseContext.AppendLine("\t}");
            databaseContext.AppendLine("}");
            FileWriter.Write(config.ClassOutputPath, String.Format("{0}DB", dbName), databaseContext.ToString());

            if (config.GenerateControllers)
            {
                var controllerGenerator = new ControllerGenerator(config, dbName.Replace(".", "_"));
                tables.ForEach(m =>
                {
                    var pkColumn = m.PrimaryKey;
                    string idType = (pkColumn == null) ? "int" : pkColumn.DataType;
                    string idName = (pkColumn == null) ? "id" : pkColumn.Name;
                    controllerGenerator.Generate(m.Name.UppercaseFirst(), idType, idName);
                });
            }
            FileWriter.Write(config.HelpersOutputPath, "PropertyCopier", scaffolder.Properties.Resources.PropertyCopier.Replace("[Namespace]", config.Namespace));
            FileWriter.Write(config.HelpersOutputPath, "SearchHelper", scaffolder.Properties.Resources.SearchHelper.Replace("[Namespace]", config.Namespace));
            FileWriter.Write(config.PropertiesOutputPath, "launchSettings.json", scaffolder.Properties.Resources.launchSettings.Replace("[Namespace]", config.Namespace).Replace("[Port]", config.ApplicationPort.ToString()).Replace("[Port2]", (config.ApplicationPort + 1).ToString()), false);
            FileWriter.Write(config.ProjectOutputPath, "appsettings.json", scaffolder.Properties.Resources.appSettings.Replace("[Namespace]", config.Namespace).Replace("[DbName]", dbName.Replace(".", "_")).Replace("[ConnectionString]", config.ConnectionString), false);
            FileWriter.Write(config.ProjectOutputPath, String.Format("{0}.csproj", config.Namespace), scaffolder.Properties.Resources.csProject, false);
            FileWriter.Write(config.ProjectOutputPath, "Program", scaffolder.Properties.Resources.Program.Replace("[Namespace]", config.Namespace));
            FileWriter.Write(config.ProjectOutputPath, "Startup", scaffolder.Properties.Resources.Startup.Replace("[Namespace]", config.Namespace).Replace("[DbName]", dbName.Replace(".", "_")));
            FileWriter.Write(config.OutputPath, String.Format("{0}.sln", config.Namespace), scaffolder.Properties.Resources.Solution.Replace("[Namespace]", config.Namespace).Replace("[AlphaGuid]", Guid.NewGuid().ToString()).Replace("[BetaGuid]", Guid.NewGuid().ToString()).Replace("[OmegaGuid]", Guid.NewGuid().ToString()), false);
        }
    }
}
