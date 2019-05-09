using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class TraitModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public List<TraitModifierModel> Modifiers
        {
            get { return Get<List<TraitModifierModel>>(); }
            set { Set(value); }
        }

        public string Description
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
