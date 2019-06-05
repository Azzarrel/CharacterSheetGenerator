using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.ModelObjects.Model
{
    public class AttributeModel : ModelObject
  {

        public SolidColorBrush Color
        {
            get { return Get<SolidColorBrush>(); }
            set { Set(value); }
        }

        public string Tag
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    public bool Special
    {
      get { return Get<bool>(); }
      set { Set(value); }
    }
  }
}
