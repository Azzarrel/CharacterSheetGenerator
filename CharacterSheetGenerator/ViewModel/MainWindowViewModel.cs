using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CharacterSheetGenerator.ViewModel
{
    class MainWindowViewModel : NotifyBase
    {
        public UserControl PresentationElement
        {
            get { return Get<UserControl>(); }
            set { Set(value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
