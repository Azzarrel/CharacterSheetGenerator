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
    public class InventoryItemModel : TemplateModel
    {

        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Quantity")]
        public double? Quantity
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Value")]
        public string Value
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Weight")]
        public double? Weight
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Place")]
        public string Place
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
