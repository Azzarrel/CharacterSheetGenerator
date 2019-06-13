using CharacterSheetGenerator.Helpers;
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
    public class ControlModel : TemplateModel
    {

       
        public UserControl Control
        {
            get { return Get<UserControl>(); }
            set { Set(value); }
        }


    }

}
