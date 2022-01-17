using Newtonsoft.Json;
using scaffolder.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace scaffolder.Generators
{
    public class SqlTypes : Dictionary<String, String>
    {
        public new String this[String type]
        {
            get
            {
                if (this.ContainsKey(type))
                {
                    return base[type];
                }
                return base["Default"];
            }
            set
            {
                base[type] = value;
            }
        }
    }

    public class TemplateGenerator
    {
        Configuration _configuration;
        ScaffoldingParameters _parameters;
        String _dbName;
        SqlTypes _types;
        public TemplateGenerator(Configuration Configuration, ScaffoldingParameters parameters, String DatabaseName)
        {
            _configuration = Configuration;
            _dbName = DatabaseName;
        }

        public bool DirectoryCheck(String root)
        {
            String[] files = Directory.GetFiles(Path.Combine(_configuration.TemplatePath, root));
            foreach(String fileName in files)
            {
                switch(fileName.Substring(fileName.Length - 4))
                {
                    //Dynamic Model Template, for listing Model and properties
                    case "pdmt":
                        break;
                    //List Model Template, for listing Models in a file
                    case "plmt":
                        break;
                    //Fixed Model Template, for controllers or where the model name is mentioned only
                    case "pfmt":
                        break;
                    //Project File Template, where only database name and namespace are needed to be setup
                    case "ppft":
                        FixedModelTemplate(root, fileName);
                        break;
                    //Misc files, just copy and paste
                    default:
                        File.Copy(Path.Combine(_configuration.TemplatePath, root, fileName), Path.Combine(_configuration.OutputPath, root, fileName));
                        break;
                }
            }

            new List<String>(Directory.GetDirectories(Path.Combine(_configuration.TemplatePath, root))).ForEach(x=> DirectoryCheck(Path.Combine(root, x)));
            return true;
        }

        public bool LoadModelEquivalence()
        {
            try
            {
                String json = File.ReadAllText(Path.Combine(_configuration.TemplatePath, "types.json"));
                _types = JsonConvert.DeserializeObject<SqlTypes>(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DynamicModelTemplate(String root, String file)
        {
            try
            {
                TemplateModelSetup modelSetup = new TemplateModelSetup();
                String readOnlyFileContents = "";
                StreamReader reader = new StreamReader(Path.Combine(_configuration.TemplatePath, root, file));
                String read = "";
                bool readingModelSetup = false;
                while((read = reader.ReadLine()) != null)
                {
                    if (readingModelSetup)
                    {
                        if (read.Contains("[PrimaryKey]::"))
                        {
                            modelSetup.PrimaryKeyAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[NotNull]::"))
                        {
                            modelSetup.NotNullAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[Null]::"))
                        {
                            modelSetup.NullAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[ForeignKey]::"))
                        {
                            modelSetup.ForeignKeyAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[Identity]::"))
                        {
                            modelSetup.IdentityAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[NonIdentity]::"))
                        {
                            modelSetup.NotIdentityAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[FKFeature]::"))
                        {
                            modelSetup.FKAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[FK]::"))
                        {
                            modelSetup.FKLines.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[Generic]::"))
                        {
                            modelSetup.GenericAnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[Property]::"))
                        {
                            modelSetup.PropertyLines.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[Varchar]::"))
                        {
                            modelSetup.VarcharAnnotations.Add(read.Substring(read.IndexOf("::") + 2));
                            continue;
                        }
                        if (read.Contains("[ContainsFK]"))
                        {
                            modelSetup.ContainsFKRelations = true;
                            continue;
                        }
                        if (read.Contains("[PropertyEqualsModels]"))
                        {
                            modelSetup.PropertyMustBeEqualToColumn = true;
                            continue;
                        }
                        if (read.Contains("[/ModelInformation]"))
                        {
                            readingModelSetup = false;
                            readOnlyFileContents += "[---MODELPROPERTIES---]\n";
                            continue;
                        }
                    }
                    else
                    {
                        if (read.Contains("[ModelInformation]"))
                        {
                            readingModelSetup = true;
                            continue;
                        }
                        readOnlyFileContents += read + "\n";
                    }
                }
                foreach (var table in _parameters.TablesToScaffold)
                {
                    String modelBuffer = "";


                    String fileContents = readOnlyFileContents.ToString();
                    fileContents = fileContents.Replace("[---MODELPROPERTIES---]", modelBuffer)
                        .Replace("[Table]", table.Name)
                        .Replace("[Namespace]", _configuration.Namespace)
                        .Replace("[Port]", _configuration.ApplicationPort.ToString())
                        .Replace("[Port2]", (_configuration.ApplicationPort + 1).ToString())
                        .Replace("[DbName]", _dbName.Replace(".", "_"))
                        .Replace("[ConnectionString]", _configuration.ConnectionString)
                        .Replace("[AlphaGuid]", Guid.NewGuid().ToString())
                        .Replace("[BetaGuid]", Guid.NewGuid().ToString())
                        .Replace("[DeltaGuid]", Guid.NewGuid().ToString())
                        .Replace("[EpsilonGuid]", Guid.NewGuid().ToString())
                        .Replace("[GammaGuid]", Guid.NewGuid().ToString())
                        .Replace("[OmegaGuid]", Guid.NewGuid().ToString())
                        .Replace("[SigmaGuid]", Guid.NewGuid().ToString());
                    File.WriteAllText(fileContents, Path.Combine(_configuration.OutputPath, root, file.Substring(0, file.Length - 5).Replace("[Table]", table.Name)));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FixedModelTemplate(String root, String file)
        {
            try
            {
                String readOnlyFileContents = File.ReadAllText(Path.Combine(_configuration.TemplatePath, root, file));
                foreach (var table in _parameters.TablesToScaffold)
                {
                    String fileContents = readOnlyFileContents.ToString();
                    fileContents = fileContents.Replace("[Table]", table.Name)
                        .Replace("[Namespace]", _configuration.Namespace)
                        .Replace("[Port]", _configuration.ApplicationPort.ToString())
                        .Replace("[Port2]", (_configuration.ApplicationPort + 1).ToString())
                        .Replace("[DbName]", _dbName.Replace(".", "_"))
                        .Replace("[ConnectionString]", _configuration.ConnectionString)
                        .Replace("[AlphaGuid]", Guid.NewGuid().ToString())
                        .Replace("[BetaGuid]", Guid.NewGuid().ToString())
                        .Replace("[DeltaGuid]", Guid.NewGuid().ToString())
                        .Replace("[EpsilonGuid]", Guid.NewGuid().ToString())
                        .Replace("[GammaGuid]", Guid.NewGuid().ToString())
                        .Replace("[OmegaGuid]", Guid.NewGuid().ToString())
                        .Replace("[SigmaGuid]", Guid.NewGuid().ToString());
                    File.WriteAllText(fileContents, Path.Combine(_configuration.OutputPath, root, file.Substring(0, file.Length - 5).Replace("[Table]", table.Name)));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ProjectFileTemplate(String root, String file)
        {
            try
            {
                String fileContents = File.ReadAllText(Path.Combine(_configuration.TemplatePath, root, file));
                fileContents = fileContents.Replace("[Namespace]", _configuration.Namespace)
                    .Replace("[Port]", _configuration.ApplicationPort.ToString())
                    .Replace("[Port2]", (_configuration.ApplicationPort + 1).ToString())
                    .Replace("[DbName]", _dbName.Replace(".", "_"))
                    .Replace("[ConnectionString]", _configuration.ConnectionString)
                    .Replace("[AlphaGuid]", Guid.NewGuid().ToString())
                    .Replace("[BetaGuid]", Guid.NewGuid().ToString())
                    .Replace("[DeltaGuid]", Guid.NewGuid().ToString())
                    .Replace("[EpsilonGuid]", Guid.NewGuid().ToString())
                    .Replace("[GammaGuid]", Guid.NewGuid().ToString())
                    .Replace("[OmegaGuid]", Guid.NewGuid().ToString())
                    .Replace("[SigmaGuid]", Guid.NewGuid().ToString());
                File.WriteAllText(fileContents, Path.Combine(_configuration.OutputPath, root, file.Substring(0, file.Length - 5)));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
