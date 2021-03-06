﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterSheetGenerator.Helpers
{
  /// <summary>
  /// Ähnlich zu der Parse-Methode by Typen gibt der Parser eine Methode mit der Objekte auch in nullable Datentypen geparst werden können
  /// </summary>
    static class Parser
    {


        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

    }

}
