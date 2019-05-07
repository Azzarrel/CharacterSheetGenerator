using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
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


        public  List<string> AttributeLinks
        {
            get { return Get<List<string>>(); }
            set { Set(value); }
        }

    }

}
