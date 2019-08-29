﻿using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model
{
    public class SkillModel : TemplateModel
    {
        [ColumnName("Name")]
        public string Name
        {
            get { return Get<string>(); }
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

        [ColumnName("Requirement")]
        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double? Base
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Value")]
        public double? Bonus
        {
            get { return Get<double?>(); }
            set
            {
                Set(value);
                SetRoutine(value);
            }
        }

        public double? Value
        {
            get { return Get<double?>(); }
            set
            {
                Set(value);
                SetToolTip(value);
            }
        }

        public string Mean
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        [ColumnName("Difficulty")]
        public string Difficulty
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        [ColumnName("Deployability")]
        public string Deployability
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Routine
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Comment")]
        public string Comment
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Category")]
        public string Category
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Grouping")]
        public string Grouping
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
        public string ToolTip
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public void SetRoutine(double? value)
        {
            Routine = "";
            if (value >= 4)
                Routine = "e";

            if (value >= 7)
                Routine = "r";

            if (value >= 10)
                Routine = "g";

            if (value >= 13)
                Routine = "m";

            if (value >= 16)
                Routine = "l";

        }
        public void SetToolTip(double? value)
        {
            ToolTip = "Gesamt: " + Value + "\n" + "Grundwert: " + Base + "\n" + "Modifier: " + Modifiers + "\n" + "Steigerung: " + Bonus;
        }
    }

}
