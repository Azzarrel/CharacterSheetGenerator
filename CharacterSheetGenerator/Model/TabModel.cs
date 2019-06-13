using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model    
{
    public class TabModel : TemplateModel
    {

       
        public ViewModelBase ViewModel
        {
            get { return Get<ViewModelBase>(); }
            set { Set(value); }
        }

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


    }

}
