using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class TraitModifierModel : NotifyBase
    {

        public string NameLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string TableLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double Value
        {
            get { return Get<double>(); }
            set
            {
                Set(value);
            }
        }

    }
}
