using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using scaffolder.Types;
using scaffolder.Helpers;

namespace scaffolder.Generators
{
    internal class EntityGenerator
    {
        private Configuration _config;

        public EntityGenerator(Configuration config)
        {
            _config = config;
        }

        private EntityGenerator() { }

        public void Generate(Table table)
        {
            var sb = new StringBuilder();
            string tableName = table.Name;
            string className = table.Name.UppercaseFirst();

            AppendUsings(sb, table);
            sb.AppendLine();
            sb.AppendFormat("namespace {0}", _config.ModelNamespace);
            sb.AppendLine();
            sb.AppendLine("{"); // Begin NameSpace
            sb.AppendFormat("\t[Table(\"{0}\")]", tableName);
            sb.AppendLine();
            sb.AppendFormat("\tpublic class {0}", className);
            sb.AppendLine();
            sb.AppendLine("\t{"); // Begin Class
            List<String> ForeignData = new List<string>();
            table.Columns.OrderBy(x=> x.ColumnOrder).ToList().ForEach(m => {
                if (m.IsForeign || (m.IsPrimaryKey && !String.IsNullOrWhiteSpace(m.ForeignTable)))
                {
                    ForeignData.Add("\t\t[JsonIgnore]");
                    ForeignData.Add("\t\tpublic " + m.ForeignTable.UppercaseFirst() + " FK_" + m.Name.UppercaseFirst() + " { get; set; }");
                    if (_config.IncludeDataAnnotations)
                    {
                        sb.AppendFormat("\t\t[ForeignKey(\"FK_{0}\")]", m.ForeignTable.UppercaseFirst());
                    }
                    sb.AppendLine();
                }
                AppendProperty(m, sb);
            });
            sb.AppendLine();
            for(int i = 0; i < ForeignData.Count; i+= 2)
            {
                sb.AppendLine(ForeignData[i]);
                sb.AppendLine(ForeignData[i + 1]);
                sb.AppendLine();
            }
             

            sb.AppendLine("\t}"); // End Class
            sb.AppendLine("}"); // End NameSpace

            FileWriter.Write(_config.ClassOutputPath, className, sb.ToString());
            
        }

        private void AppendUsings(StringBuilder sb, Table tableInfo)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine("using Newtonsoft.Json;");
        }

        private void AppendProperty(Column column, StringBuilder sb)
        {
            if (_config.IncludeDataAnnotations)
            {
                if (column.IsPrimaryKey)
                {
                    sb.AppendLine("\t\t[Key]");
                    if (column.Identity)
                    {
                        sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                    }
                    else
                    {
                        sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.None)]");
                    }
                }
                if (!column.IsPrimaryKey && !column.Nullable) // Not adding required to primary key to allow inserting of new records.
                {
                    sb.AppendLine("\t\t[Required]");
                }
                sb.AppendFormat("\t\t[Column(\"{0}\")]\n", column.Name);
            }
            
            sb.AppendFormat("\t\tpublic {0} {1} {{ get; set; }}\n\n", column.DataType, column.Name);
        }
    }
}
