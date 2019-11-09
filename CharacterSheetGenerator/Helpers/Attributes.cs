﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CharacterSheetGenerator.Helpers
{
        public class ColumnNameAttribute : Attribute
        {
            public string Name { get; set; }

            public ColumnNameAttribute(string s)
            {
                Name = s;
            }
        }
    public class LangColumnNameAttribute : Attribute
    {
        public string Default { get; set; }
        public string LangName { get; set; }

        public LangColumnNameAttribute(string def, string lang)
        {
            Default = def;
            LangName = lang;


        }
    }

    public class SaveDataAttribute : Attribute
    {
        public SaveDataAttribute()
        {
        }
    }
}