﻿using CharacterSheetGenerator.Helpers;
using System.Windows.Media;


namespace CharacterSheetGenerator.Model
{
    public class AttributeModel : TemplateModel
    {
        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Value")]
        public double Base
        {
            get { return Get<double>(); }
            set { Set(value); }
        }


        public double Value
        {
            get { return Get<double>(); }
            set { Set(value); }
        }


        public double Modifiers
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        [ColumnName("Key")]
        public int Key
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        [ColumnName("Color")]
        public SolidColorBrush Color
        {
            get { return Get<SolidColorBrush>(); }
            set { Set(value); }
        }

        [ColumnName("Tag")]
        public string Tag
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        
        public bool Special
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
