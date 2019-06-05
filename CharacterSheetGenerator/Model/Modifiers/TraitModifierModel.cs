using CharacterSheetGenerator.Helpers;
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
    public class TraitModifierModel : NotifyBase
    {

        public ModifierModel Modifier
        {
            get { return Get<ModifierModel>(); }
            set
            {
                Set(value);
                Modifier.TypeLink = Types.FirstOrDefault();
                OnPropertyChanged("TypeLink");
                OnPropertyChanged("Types");
            }
        }

        public string NameLink
        {
            get { return Modifier?.NameLink; }
        }

        public string TypeLink
        {
            get { return Modifier?.TypeLink; }
            set { Modifier.TypeLink = value; }
        }

        public ObservableCollection<string> Types
        {
            get { return Modifier?.Types; }
        }

        public double Value
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public int TraitLink
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

    }

}
