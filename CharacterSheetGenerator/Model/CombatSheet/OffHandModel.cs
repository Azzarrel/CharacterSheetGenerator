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
    public class OffHandModel : TemplateModel
    {
        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Strenght")]
        public int? Strenght
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Toughness")]
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

        public double? BlockBase
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

        [ColumnName("Break")]
        public int? Break
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

    }
}
