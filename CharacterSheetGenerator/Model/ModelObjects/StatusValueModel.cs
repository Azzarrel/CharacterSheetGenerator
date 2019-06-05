using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.ModelObjects.Model 
{
    public class StatusValueModel : ModelObject
    {


        public double Standard
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Bonus
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public  ObservableCollection<string> AttributeLinks
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

    }

}
