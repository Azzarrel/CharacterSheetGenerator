using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.CombatSheet.Model 
{
    public class ArmorModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public int? Head
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Torso
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? LeftArm
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? RightArm
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? LeftLeg
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? RightLeg
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Toughness
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Restriction
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Slow
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Break
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }


    }
}
