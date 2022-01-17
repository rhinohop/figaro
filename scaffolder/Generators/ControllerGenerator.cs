using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using scaffolder.Helpers;
using scaffolder.Types;

namespace scaffolder.Generators
{
    internal class ControllerGenerator
    {
        private Configuration _config;
        private String _dbName;

        private ControllerGenerator() { }

        public ControllerGenerator(Configuration config, String database)
        {
            _config = config;
            _dbName = database;
        }

        public void Generate(string modelName, string idType, string idName)
        {
            var sb = new StringBuilder();

            AppendUsings(sb);

            sb.AppendFormat("\nnamespace {0}\n", _config.ControllerNamespace);
            sb.AppendLine("{"); // Begin NameSpace
            sb.AppendLine("\t[Route(\"api/[controller]\")]");
            sb.AppendFormat("\tpublic class {0}Controller : Controller", modelName);
            sb.AppendLine("\n\t{"); // Begin Controller Class

            AppendIndex(sb, modelName);
            AppendConstructor(sb, modelName);
            AppendCreate(sb, modelName);
            AppendDetail(sb, modelName, idType);
            AppendEdit(sb, modelName, idName);
            AppendDelete(sb, modelName, idType);

            sb.AppendLine("\t}"); // End Class
            sb.AppendLine("}"); // End NameSpace

            FileWriter.Write(_config.ControllerOutputPath, modelName + "Controller", sb.ToString());
        }

        private void AppendConstructor(StringBuilder sb, String modelName)
        {
            sb.AppendFormat("\t\tprivate {0}DB db;", _dbName);
            sb.AppendLine("");
            sb.AppendFormat("\t\tpublic {0}Controller({1}DB database)", modelName, _dbName);
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tdb = database;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
        }

        private void AppendUsings(StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendFormat("using {0}.Helpers;\n", _config.Namespace);
            sb.AppendFormat("using {0}.Models;\n", _config.Namespace);
        }

        private void AppendIndex(StringBuilder sb, string modelName)
        {
            sb.AppendLine("\t\t[HttpGet]");
            sb.AppendLine("\t\tpublic async Task<ActionResult> Index(Int32? pagina = 1, Int32? cantidad = 30, String field = null, String searchTerm = null, String searchOperator = null)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tActionResult result = await Task.Run(() => Selection(pagina.Value, cantidad.Value, field, searchTerm, searchOperator));");
            sb.AppendLine("\t\t\treturn result;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\tpublic ActionResult Selection(Int32 page, Int32 quantity, String field, String searchTerm, String searchOperator)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tInt32 currentPage = page - 1;");
            sb.AppendFormat("\t\t\t\tvar source = SearchHelper<{0}>.SearchFiltering(db.{0}{1}.AsQueryable(), field, searchTerm, searchOperator);\n", modelName, _config.UseLogicDelete ? String.Format(".Where(x=> x.{0} == true)", _config.DeleteField) : "");
            sb.AppendLine("\t\t\t\tvar results = source.Where(x=> x.estatus == true).Skip(currentPage * quantity).Take(quantity).AsQueryable();");
            sb.AppendLine("\t\t\t\tif (results.Count() == 0)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\treturn NoContent();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t\treturn Ok(results);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch (Exception e)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn StatusCode(500, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
        }

        private void AppendCreate(StringBuilder sb, string modelName)
        {
            
            sb.AppendLine("\t\t[HttpPost]");
            sb.AppendLine("\t\tpublic async Task<ActionResult> Create([FromBody]object json)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t\t{0} model = JsonConvert.DeserializeObject<{0}>(json.ToString());\n", modelName);
            sb.AppendLine("\t\t\tmodel.estatus = true;");
            sb.AppendLine("\t\t\tif (!ModelState.IsValid)");
            sb.AppendLine("\t\t\t\treturn BadRequest();");
            sb.AppendLine("");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendFormat("\t\t\t\tawait db.{0}.AddAsync(model);\n", modelName);
            sb.AppendLine("\t\t\t\tawait db.SaveChangesAsync();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch (Exception ex)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn StatusCode(500, ex);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn Ok();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
        }

        private void AppendDetail(StringBuilder sb, string modelname, string idType)
        {
            sb.AppendLine("\t\t[HttpGet(\"{id}\")]");
            sb.AppendFormat("\t\tpublic async Task<ActionResult> Find({0} id)\n", idType + (new String[] { "String" }.Contains(idType) ? "" : "?"));
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif (id == null)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn BadRequest();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendFormat("\t\t\t\tvar item = await db.{0}.FindAsync({1});\n", modelname, (new String[] { "String" }.Contains(idType) ? "id" : "id.Value"));
            sb.AppendLine("\t\t\t\tif (item == null)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\treturn NotFound();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t\treturn Ok(item);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch (Exception e)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn StatusCode(500, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");   
        }

        private void AppendEdit(StringBuilder sb, string modelName, string idName)
        {
            sb.AppendLine("\t\t[HttpPut]");
            sb.AppendLine("\t\tpublic async Task<ActionResult> Edit([FromBody] object json)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendFormat("\t\t\t\t{0} model = JsonConvert.DeserializeObject<{0}>(json.ToString());\n", modelName);
            sb.AppendFormat("\t\t\t\t{0} original = db.{0}.Find(model.{1});\n", modelName, idName);
            sb.AppendLine("\t\t\t\tmodel.estatus = original.estatus;");
            sb.AppendLine("\t\t\t\tPropertyCopier.CopyPropertyValues(model, original);");
            sb.AppendLine("\t\t\t\tModelState.Clear();");
            sb.AppendLine("\t\t\t\tTryValidateModel(original);");
            sb.AppendLine("\t\t\t\tif (!ModelState.IsValid)");
            sb.AppendLine("\t\t\t\t\treturn BadRequest();");
            sb.AppendLine("\t\t\t\tdb.Entry(original).State = Microsoft.EntityFrameworkCore.EntityState.Modified;");
            sb.AppendLine("\t\t\t\tawait db.SaveChangesAsync();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch (Exception ex)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn StatusCode(500, ex);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn Ok();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
        }

        private void AppendDelete(StringBuilder sb, string modelName, string idType)
        {
            sb.AppendLine("\t\t[HttpDelete(\"{id}\")]");
            sb.AppendFormat("\t\tpublic async Task<ActionResult> Delete({0} id)\n", idType + (new String[] { "String" }.Contains(idType) ? "" : "?"));
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif (id == null)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn BadRequest();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendFormat("\t\t\t\tvar item = await db.{0}.FindAsync(id.Value);\n", modelName);
            sb.AppendLine("\t\t\t\tif (item == null)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\treturn NotFound();");
            sb.AppendLine("\t\t\t\t}");
            if (_config.UseLogicDelete)
            {
                sb.AppendFormat("\t\t\t\titem.{0} = false;\n", _config.DeleteField);
                sb.AppendLine("\t\t\t\tdb.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;");
            }
            else
            {
                sb.AppendFormat("\t\t\t\tdb.{0}.Remove(item);\n", modelName);
            }
            sb.AppendLine("\t\t\t\tawait db.SaveChangesAsync();");
            sb.AppendLine("\t\t\t\treturn Ok();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch (Exception e)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\treturn StatusCode(500, e);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
        }
    }
}
