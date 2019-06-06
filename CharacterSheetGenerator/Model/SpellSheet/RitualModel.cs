using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model
{
    public class RitualModel : TemplateModel
    {
        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Type")]
        public string Type
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Requirement")]
        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Value")]
        public int? Value
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Description")]
        public string Description
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Duration")]
        public string Duration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
