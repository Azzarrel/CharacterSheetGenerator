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
    public class TraitModel : TemplateModel
    {

        [ColumnName("Key")]
        public int Key
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Description")]
        public string Description
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
