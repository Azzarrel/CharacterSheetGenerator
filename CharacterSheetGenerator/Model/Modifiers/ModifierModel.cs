using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
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
    public class ModifierModel : TemplateModel
    {


        public string NameLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string TypeLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public ObservableCollection<string> Types
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

        public ModifierModel()
        {
            Types = new ObservableCollection<string>();
        }

    }

}
