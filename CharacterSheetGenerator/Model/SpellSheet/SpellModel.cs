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
        public int? Value
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Description")]
        public string Description
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
        [ColumnName("Ticks")]
        public string Ticks
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

    }
}
