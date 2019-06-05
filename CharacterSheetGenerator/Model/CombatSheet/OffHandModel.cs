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
    public class OffHandModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public int? Strenght
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Toughness
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public double? AttackBase
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? AttackBonus
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? AttackTotal
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? BlockBase
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? BlockBonus
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public double? BlockTotal
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        public int? Break
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }


    }
}
