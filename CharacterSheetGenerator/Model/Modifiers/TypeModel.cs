using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class TypeModel : NotifyBase
    {



        public string TypeLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }

}
