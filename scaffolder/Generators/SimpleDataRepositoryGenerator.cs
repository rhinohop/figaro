using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Types;

namespace scaffolder.Generators
{
    internal class SimpleDataRepositoryGenerator : IRepositoryGenerator
    {
        public string GetRepositoryMethods(Table tableInfo)
        {
            var sb = new StringBuilder();
            string primaryKeyColumnName = "Id";   // sane default, just in case
            string primaryKeyColumnType = "int?"; // sane defailt, just in case
            string className = tableInfo.Name.UppercaseFirst();
            string objectName = tableInfo.Name.ToLowerInvariant();

            var pkResult = tableInfo.Columns.Find(m => m.IsPrimaryKey == true);
            if (pkResult != null)
            {
                primaryKeyColumnName = pkResult.Name;
                primaryKeyColumnType = pkResult.DataType; 
            }

            sb.AppendLine("\n\t\t/***************************************/");
            sb.AppendLine(GetGetAll(className));
            sb.AppendLine(GetGet(className, primaryKeyColumnType, primaryKeyColumnName));
            sb.AppendLine(GetCreate(className, objectName, primaryKeyColumnName));
            sb.AppendLine(GetSave(className, objectName));
            sb.AppendLine(GetDelete(className, primaryKeyColumnType, primaryKeyColumnName));

            return sb.ToString();
        }

        public string GetUsings(Table tableInfo)
        {
            return "using Simple.Data;";
        }

        private string GetCreate(string className, string objectName, string pkName)
        {
            string code = @"
        public static void Create({0} {1})
        {{
            {1}.{2} = null;
            Database.Open().{0}s.Insert({2});
        }}";

            return string.Format(code, className, objectName, pkName);
        }

        private string GetGet(string className, string pkType, string pkName)
        {
            string code = @"
        public static {0} Get({1} id)
        {{
            return ({0})Database.Open().{0}s.FindBy{2}(id);
        }}";

            return string.Format(code, className, pkType, pkName);
        }

        private string GetGetAll(string className)
        {
            string code = @"
        public static IEnumerable<{0}> GetAll()
        {{
            return Database.Open().{0}s.All.Cast<{0}>();
        }}";

            return string.Format(code, className);
        }

        private string GetSave(string className, string objectName)
        {
            string code = @"
        public static void Save({0} {1})
        {{
            Database.Open().{0}s.Update({1});
        }}";

            return string.Format(code, className, objectName);
        }

        private string GetDelete(string className, string pkType, string pkName)
        {
            string code = @"
        public static void Delete({0} id)
        {{
            Database.Open().{1}s.DeleteBy{2}(id);
        }}";

            return string.Format(code, pkType, className, pkName);
        }
    }
}
