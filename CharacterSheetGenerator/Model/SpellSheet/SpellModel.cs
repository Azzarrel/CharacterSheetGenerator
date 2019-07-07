using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model.CombatSheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model 
{
    public class SpellModel : TemplateModel
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

        [ColumnName("Requirement")]
        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Value")]
        public int? Base
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        public int? Value
        {
            get { return Get<int?>(); }
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

        [ColumnName("MagicDamage")]
        public string MagicDamage
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

        [ColumnName("Range")]
        public string Range
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Duration")]
        public string Duration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Mana")]
        public string Mana
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Ticks
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("AttackBonus")]
        public bool AttackBonus
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
