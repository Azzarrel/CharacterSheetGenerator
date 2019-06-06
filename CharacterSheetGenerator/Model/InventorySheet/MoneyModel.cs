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
    public class MoneyModel : TemplateModel
    {

        [ColumnName("Gold")]
        public double? Gold
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Silver")]
        public double? Silver
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Copper")]
        public double? Copper
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Iron")]
        public double? Iron
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Gems")]
        public string Gems
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Artifacts")]
        public string Artifacts
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Rest")]
        public string Rest
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
