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
    public class InventoryItemModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double Quantity
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Value
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Weight
        {
            get { return Get<double>(); }
            set { Set(value); }
        }


        public string Place
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
