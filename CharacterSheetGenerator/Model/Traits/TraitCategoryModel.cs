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

namespace CharacterSheetGenerator.Traits.Model
{
    public class TraitCategoryModel : TemplateModel
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


        public string TraitTexts
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public ObservableCollection<TraitModel> Traits
        {
            get { return Get<ObservableCollection<TraitModel>>(); }
            set { Set(value); }
        }

        public int Key
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

    }

}
