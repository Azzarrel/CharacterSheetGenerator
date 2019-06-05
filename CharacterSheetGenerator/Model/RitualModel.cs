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
    public class RitualModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Type
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public int? Value
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public string Description
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Duration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
