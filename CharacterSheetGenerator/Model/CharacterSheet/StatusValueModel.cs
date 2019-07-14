using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model 
{
    public class StatusValueModel : TemplateModel
    {
        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Base")]
        public double Base
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Value
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Modifiers
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        [ColumnName("StatusValues_Id")]
        public int Key
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        public double Standard
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        [ColumnName("Bonus")]
        public double Bonus
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        [ColumnName("SVAttributeLink")]
        public  ObservableCollection<string> AttributeLinks
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

    }

}
