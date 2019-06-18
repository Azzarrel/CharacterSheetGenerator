using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model.CombatSheet
{
    public class RangedWeaponModel : TemplateModel
    {

        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Weapons")]
        public WeaponModel Weapons
        {
            get { return Get<WeaponModel>(); }
            set { Set(value); }
        }

        [ColumnName("Damage")]
        public string Damage
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Impulse")]
        public string Impulse
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("ArmorPenetration")]
        public string ArmorPenetration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("AttackBonus")]
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

        [ColumnName("BlockBonus")]
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

        [ColumnName("Load")]
        public int? Load
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Ticks")]
        public int? Ticks
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Break")]
        public int? Break
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Stamina")]
        public int? Stamina
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("StaminaLoad")]
        public int? StaminaLoad
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Range")]
        public string Range
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
