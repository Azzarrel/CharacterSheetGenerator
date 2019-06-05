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
    public class MeleeWeaponModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public WeaponModel Weapons
        {
            get { return Get<WeaponModel>(); }
            set { Set(value); }
        }

        public string Damage
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Impulse
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string ArmorPenetration
        {
            get { return Get<string>(); }
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

        public int? Ticks
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Break
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public string Range
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
