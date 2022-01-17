using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scaffolder.Types
{
    public class TemplateModelSetup
    {
        public List<String> PrimaryKeyAnnotations { get; set; }
        public List<String> NotIdentityAnnotations { get; set; }
        public List<String> IdentityAnnotations { get; set; }
        public List<String> ForeignKeyAnnotations { get; set; }
        public List<String> VarcharAnnotations { get; set; }
        public List<String> NotNullAnnotations { get; set; }
        public List<String> NullAnnotations { get; set; }
        public List<String> GenericAnotations { get; set; }
        public List<String> PropertyLines { get; set; }
        public List<String> FKLines { get; set; }
        public List<String> FKAnnotations { get; set; }
        public Boolean ContainsFKRelations { get; set; }

        /// <summary>
        /// Establish if the column must be same name as column and it cannot be uppercased
        /// </summary>
        public Boolean PropertyMustBeEqualToColumn { get; set; }

        public TemplateModelSetup()
        {
            PrimaryKeyAnnotations = new List<String>();
            NotIdentityAnnotations = new List<String>();
            IdentityAnnotations = new List<String>();
            ForeignKeyAnnotations = new List<String>();
            VarcharAnnotations = new List<String>();
            NotNullAnnotations = new List<String>();
            NullAnnotations = new List<String>();
            GenericAnotations = new List<String>();
            PropertyLines = new List<String>();
            FKLines = new List<String>();
            FKAnnotations = new List<String>();
            ContainsFKRelations = false;
            PropertyMustBeEqualToColumn = false;
        }
    }
}
