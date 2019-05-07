using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace CharacterSheetGenerator
{
    class ColorHandler
    {


        public static Color IntToColor(int RGB)
        {
            byte r = (byte)((RGB & 0xFF0000) >> 16);
            byte g = (byte)((RGB & 0x00FF00) >> 8);
            byte b = (byte)((RGB & 0x0000FF));

            return Color.FromRgb(r, g, b);
        }
    }
}