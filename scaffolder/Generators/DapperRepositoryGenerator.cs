using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scaffolder.Types;

namespace scaffolder.Generators
{
    internal class DapperRepositoryGenerator : IRepositoryGenerator
    {
        public string GetUsings(Table tableInfo)
        {
            return string.Format("using Dapper;\nusing {0};", tableInfo.AdoAdapterNamespace);
        }

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

            var columnNames =  tableInfo.Columns.Select(m => { return m.Name; });

            sb.AppendLine("\n\t\t/***************************************/");
            sb.AppendLine(GetGetAllMethod(className, columnNames, tableInfo.AdoAdapterConnectionClassName, tableInfo.Schema));
            sb.AppendLine(GetGetMethod(className, primaryKeyColumnName, primaryKeyColumnType, columnNames, tableInfo.AdoAdapterConnectionClassName, tableInfo.Schema));
            sb.AppendLine(GetCreateMethod(className, objectName, primaryKeyColumnName, columnNames, tableInfo.AdoAdapterConnectionClassName, tableInfo.Schema));
            sb.AppendLine(GetSaveMethod(className, objectName, primaryKeyColumnName, columnNames, tableInfo.AdoAdapterConnectionClassName, tableInfo.Schema));
            sb.AppendLine(GetDeleteMethod(className, primaryKeyColumnName, primaryKeyColumnType, tableInfo.AdoAdapterConnectionClassName, tableInfo.Schema));

            return sb.ToString(); 
        }

        private string GetDeleteMethod(string className, string pkName, string pkType, string dbConnectionClass, string schema)
        {
            string code = @"
        public static void Delete({0} id)
        {{
            using (var con = new {1}(_connectionString))
            {{
                var query = ""delete from {4}.{2} where {3} = @Id;"";
                con.Open();
                return con.Execute(query, new {{ Id = id }});
            }}
        }}";

            return string.Format(code, pkType, dbConnectionClass, className, pkName, schema);
        }

        private string GetGetMethod(string className, string pkName, string pkType, IEnumerable<string> columnNames, string dbConnectionClass, string schema)
        {
            string code = @"
        public static {0} Get({1} id)
        {{
            using (var con = new {2}(_connectionString))
            {{
                var query = ""select {3} from {5}.{0} where {4} = @Id;"";
                con.Open();
                return con.Query<{0}>(query, new {{ Id = id }}).First();
            }}
        }}";

            return string.Format(code, className, pkType, dbConnectionClass, string.Join(", ", columnNames), pkName, schema);
        }

        private string GetGetAllMethod(string className, IEnumerable<string> columnNames, string dbConnectionClass, string schema)
        {
            string code = @"
        public static IEnumerable<{0}> GetAll()
        {{
            using (var con = new {1}(_connectionString))
            {{
                var query = ""select {2} from {3}.{0};"";
                con.Open();
                return con.Query<{0}>(query);
            }}
        }}";
            
            return string.Format(code, className, dbConnectionClass, string.Join(", ", columnNames), schema);
        }

        private string GetSaveMethod(string className, string objectName, string pkName, IEnumerable<string> columnNames, string dbConnectionClass, string schema)
        {
            string code = @"
		public static void Save({0} {1})
		{{
			using (var con = new {2}(_connectionString))
			{{
				var query = ""update {0} set {6}.{3} where {4} = @{4};"";
				con.Open();
				return con.Execute(query, new {{ {5} }});
			}}
		}}";
            var setList = new StringBuilder();      // {3}
            var parameterList = new StringBuilder();// {4}

            foreach (var column in columnNames)
            {
                if (column != pkName)
                {
                    setList.AppendFormat(" {0} = @{0},", column, objectName);
                }

                parameterList.AppendFormat(" {0} = {1}.{0},", column, objectName);
            }
            
            // Now we do a dirty hack to remove that extra comma at the end of each string
            setList.Remove(setList.Length - 1, 1);
            parameterList.Remove(parameterList.Length - 1, 1);
 
            return string.Format(code, className, objectName, dbConnectionClass, setList.ToString(), pkName, parameterList.ToString(), schema);
        }

        private string GetCreateMethod(string className, string objectName, string pkName, IEnumerable<string> columnNames, string dbConnectionClass, string schema)
        {
            string code = @"
        public static void Create({0} {1})
        {{
            using (var con = new {2}(_connectionString))
            {{
                var query = ""insert into {6}.{0} ({3}) values ({4});"";
                con.Open();
                return con.Execute(query, new {{ {5} }});
            }}
        }}";

            var columnList = new StringBuilder();   // {3}
            var valueList = new StringBuilder();    // {4}
            var parameterList = new StringBuilder();// {5}
            
            foreach (var column in columnNames)
            {
                if (column == pkName)
                    continue; // we want to ignore pk so that autoincrementing fields get set properly

                columnList.AppendFormat(" {0},", column);
                valueList.AppendFormat(" @{0},", column);
                parameterList.AppendFormat(" {0} = {1}.{0},", column, objectName);
            }
            
            // Now we do a dirty hack to remove that extra comma at the end of each string
            columnList.Remove(columnList.Length - 1, 1);
            valueList.Remove(valueList.Length - 1, 1);
            parameterList.Remove(parameterList.Length - 1, 1);

            return string.Format(code, className, objectName, dbConnectionClass, columnList.ToString(), valueList.ToString(), parameterList.ToString(), schema); 
        }
    }
}
