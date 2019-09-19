using System;
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
    public class ColumnLangNameAttribute : Attribute
    {
        public string Default { get; set; }
        public string LangName { get; set; }

        public ColumnLangNameAttribute(string def, string lang)
        {
            Default = def;
            LangName = lang;


        }
    }
}