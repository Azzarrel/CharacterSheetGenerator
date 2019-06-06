using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.CombatSheet.Model 
{
    public class ArmorModel : TemplateModel
    {
        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Head")]
        public int? Head
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Torso")]
        public int? Torso
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("LeftArm")]
        public int? LeftArm
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("RightArm")]
        public int? RightArm
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("LeftLeg")]
        public int? LeftLeg
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("RightLeg")]
        public int? RightLeg
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

        [ColumnName("Restriction")]
        public int? Restriction
        {
            get { return Get<int?>(); }
            set { Set(value); }
        }

        [ColumnName("Slow")]
        public int? Slow
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


    }
}
