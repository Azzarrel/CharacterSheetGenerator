using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.ModelObjects.Model
{
    public class ModelObject : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

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

        public int Key
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

    }
}
