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
    public class WeaponModel : TemplateModel
    {

        [ColumnLangName("Name_ger", "Name_ger")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Mode
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double AttackBase
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AttackStandard
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AttackModifier
        {
            get { return Get<double>(); }
            set { Set(value); }
        }


        [ColumnName("AttackBonus")]
        public double AttackBonus
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AttackTotal
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockBase
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockStandard
        {
            get { return Get<double>(); }
            set { Set(value); }
        }


        [ColumnName("BlockModifier")]
        public double BlockModifier
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockBonus
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockTotal
        {
            get { return Get<double>(); }
            set { Set(value); }
        }


        [ColumnName("AttributeLink")]
        public string AttributeLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Stamina")]
        public int Stamina
        {
            get { return Get<int>(); }
            set { Set(value); }
        }


        [ColumnName("Initiative")]
        public int Initiative
        {
            get { return Get<int>(); }
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

        [ColumnName("Reload")]
        public double Reload
        {
            get { return Get<double>(); }
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
