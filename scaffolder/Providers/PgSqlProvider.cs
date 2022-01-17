using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using scaffolder.Types;

namespace scaffolder.Providers
{
    internal class PgSqlProvider : ISqlProvider
    {
        public string ConnectionString { get; set; }

        public PgSqlProvider() {}

        /// <summary>
        /// Test connection to the database using the connection string provided in the class constructor.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        public bool TestConnection()
        {
            var conn = new NpgsqlConnection(ConnectionString);
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

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                string query = "select table_schema || '.' || table_name from information_schema.tables where table_schema = 'public' order by table_schema, table_name";
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
            string query = @"select 
	                            cols.column_name as Name, 
	                            cols.data_type as Type, 
	                            CAST(cols.is_nullable as boolean) as IsNullable,
	                            CAST(case when pk.COLUMN_NAME is NULL then 0 else 1 end as boolean) as IsPrimaryKey
                            from 
	                            information_schema.columns cols left join
	                            (select kcu.TABLE_SCHEMA, kcu.TABLE_NAME, kcu.CONSTRAINT_NAME, tc.CONSTRAINT_TYPE, kcu.COLUMN_NAME, kcu.ORDINAL_POSITION
		                            from INFORMATION_SCHEMA.TABLE_CONSTRAINTS as tc
		                            join INFORMATION_SCHEMA.KEY_COLUMN_USAGE as kcu
			                            on kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
			                            and kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
			                            and kcu.TABLE_SCHEMA = tc.TABLE_SCHEMA
			                            and kcu.TABLE_NAME = tc.TABLE_NAME
		                            where tc.CONSTRAINT_TYPE = 'PRIMARY KEY') 
	                                as pk	on cols.TABLE_SCHEMA = pk.TABLE_SCHEMA 
		                                and cols.TABLE_NAME = pk.TABLE_NAME
		                                and cols.COLUMN_NAME = pk.COLUMN_NAME
                            where
	                            cols.TABLE_SCHEMA = @schema and
	                            cols.TABLE_NAME = @name
                            order by
	                            cols.ordinal_position";

            var tables = new List<Table>(tableList.Count());

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                foreach (var value in tableList)
                {
                    var split = value.Split('.');
                    var table = new Table();
                    table.AdoAdapterConnectionClassName = "NpgsqlConnection";
                    table.AdoAdapterNamespace = "Npgsql";
                    table.Name = split[1];
                    table.Schema = split[0];
                    table.Columns = conn.Query<Column>(query, new { schema = table.Schema, name = table.Name }).ToList<Column>();
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
                case "int2":
                    result = isNullable ? "short?" : "short";
                    break;

                case "int":
                case "int4":
                case "integer":
                    result = isNullable ? "int?" : "int";
                    break;

                case "bigint":
                case "bigserial":
                case "int8":
                case "serial8":
                    result = isNullable ? "long?" : "long";
                    break;

                case "boolean":
                case "bool":
                    result = isNullable ? "bool?" : "bool";
                    break;

                case "float8":
                case "double precision":
                    result = isNullable ? "double?" : "double";
                    break;

                case "real":
                case "float4":
                    result = isNullable ? "single?" : "single";
                    break;

                case "interval":
                case "time":
                case "time without time zone":
                case "time with time zone":
                    result = isNullable ? "TimeSpan?" : "TimeSpan";
                    break;

                case "char":
                case "character":
                case "character varying":
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
                case "bytea":
                case "varbinary":
                case "varbinarymax":
                    result = isNullable ? "byte?[]" : "byte[]";
                    break;

                case "date":
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                case "timestamp without time zone":
                case "timestamp with time zone":
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
