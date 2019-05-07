﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class TraitCategoryModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string TraitTexts
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public List<TraitModel> Traits
        {
            get { return Get<List<TraitModel>>(); }
            set { Set(value); }
        }

     
    }

}
