using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class MoneyModel : NotifyBase
    {

        public double? Gold
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? Silver
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? Copper
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? Iron
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public string Gems
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Artifacts
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Rest
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
