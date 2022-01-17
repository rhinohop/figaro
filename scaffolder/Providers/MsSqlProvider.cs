using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using scaffolder.Types;

namespace scaffolder.Providers
{
    internal class MsSqlProvider : ISqlProvider
    {
        public string ConnectionString { get; set; }

        public MsSqlProvider () {}

        /// <summary>
        /// Test connection to the database using the connection string provided in the class constructor.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        public bool TestConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                conn.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets all available table names from the database.
        /// </summary>
        /// <returns>Enumerable containing all tables in the database in schema.table format.</returns>
        public IEnumerable<string> GetAvailableTables()
        {
            if (!TestConnection())
                throw new InvalidOperationException("Failed to connect to the database.");
            
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "select table_schema + '.' + table_name from information_schema.tables where TABLE_TYPE = 'BASE TABLE' order by table_schema, table_name";
                conn.Open();
                return conn.Query<string>(query);
            }
        }

        /// <summary>
        /// Get full table info for a list of tables.
        /// </summary>
        /// <param name="tableList">Enumerable containing the names of the tables to be retrieved.</param>
        /// <returns>List of Table with full column information.</returns>
        public List<Table> GetFullTableInfo(IEnumerable<string> tableList)
        { 
            var tables = new List<Table>(tableList.Count());

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                //Thanks to https://www.youtube.com/watch?v=b7ueMlAZpE4 for the more accurate fk logic
                string query = @"select * from (
Select DISTINCT
	r.ORDINAL_POSITION as ColumnOrder,
	r.column_name as Name,
	r.data_type as DataType, 
	r.CHARACTER_MAXIMUM_LENGTH as DataLength, 
	(Case when r.IS_NULLABLE = 'YES' then 1 else 0 END) as Nullable, 
	COLUMNPROPERTY(object_id(r.TABLE_NAME), r.COLUMN_NAME, 'IsIdentity') as 'Identity', 
	isnull(w.destination_table, '') as ForeignTable, 
	isnull(w.destination_column, '') as ForeignKey, 
	(case when t.constraint_type = 'PRIMARY KEY' then 1 when t.constraint_type = 'FOREIGN KEY' then 2 when t.CONSTRAINT_TYPE = 'UNIQUE' then 3 else 0 END) as 'KeyType' 
from information_schema.columns r 
	left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE c on r.column_name = c.column_name and r.table_name = c.table_name 
	left join INFORMATION_SCHEMA.TABLE_CONSTRAINTS t on c.table_name = t.table_name and c.constraint_name = t.constraint_name
	left join (

		SELECT DISTINCT
			object_name(FK.parent_object_id) as table_name,
			cf.name as column_name,
			object_name(FK.referenced_object_id) destination_table,
			c.name as destination_column
		FROM sys.foreign_keys AS FK
			INNER JOIN sys.foreign_key_columns AS FKC
				ON FK.OBJECT_ID = FKC.constraint_object_id
			INNER JOIN sys.columns c
				ON c.OBJECT_ID = FKC.referenced_object_id
					AND c.column_id = FKC.referenced_column_id
					INNER JOIN sys.columns cf
				ON cf.OBJECT_ID = FKC.parent_object_id
					AND cf.column_id = FKC.parent_column_id
		WHERE object_name(FK.parent_object_id) = @table
	) as w on c.table_name = w.table_name and c.column_name = w.column_name 
where r.table_name = @table) z where z.KeyType != 3 order by ColumnOrder;";
                conn.Open();

                foreach (var value in tableList)
                {
                    var split = value.Split('.');
                    var table = new Table();
                    table.AdoAdapterNamespace = "System.Data.SqlClient";
                    table.AdoAdapterConnectionClassName = "SqlConnection";
                    table.Name = split[1];
                    table.Schema = split[0];
                    table.Columns = conn.Query<Column>(query, new { table = table.Name }).ToList<Column>();
                    table.Columns.ForEach(m => m.DataType = GetNetDataType(m.DataType, m.Nullable));
                    tables.Add(table);
                }                
            }

            return tables;
        }

        private string GetNetDataType(string dataType, bool isNullable)
        {
            string result;

            switch (dataType.ToLower())
            {
                case "smallint":
                    result = isNullable ? "short?" : "short";
                    break;

                case "int":
                    result = isNullable ? "int?" : "int";
                    break;

                case "bigint":
                    result = isNullable ? "long?" : "long";
                    break;

                case "bit":
                    result = isNullable ? "bool?" : "bool";
                    break;

                case "float":
                    result = isNullable ? "double?" : "double";
                    break;

                case "real":
                    result = isNullable ? "single?" : "single";
                    break;

                case "tinyint":
                    result = isNullable ? "byte?" : "byte";
                    break;

                case "uniqueidentifier":
                    result = isNullable ? "Guid?" : "Guid";
                    break;

                case "time":
                    result = isNullable ? "TimeSpan?" : "TimeSpan";
                    break;

                case "datetimeoffset":
                    result = isNullable ? "DateTimeOffset?" : "DateTimeOffset";
                    break;

                case "nchar":
                case "ntext":
                case "char":
                case "nvarchar":
                case "nvarcharmax":
                case "text":
                case "varchar":
                case "varcharmax":
                case "xml":
                    result = "string";
                    break;

                case "geography":
                case "geometry":
                case "hierarchyid":
                case "sql_variant":
                    result = "object";
                    break;

                case "binary":
                case "image":
                case "timestamp":
                case "varbinary":
                case "varbinarymax":
                    result = "byte[]";
                    break;

                case "date":
                case "datetime":
                case "smalldatetime":
                    result = isNullable ? "DateTime?" : "DateTime";
                    break;

                case "money":
                case "numeric":
                case "decimal":
                case "smallmoney":
                    result = isNullable ? "decimal?" : "decimal";
                    break;

                default:
                    result = "object";
                    break;
            }
            return result;
        }
    }
}
