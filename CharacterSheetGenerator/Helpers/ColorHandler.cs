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

        public static int ColorToInt(Color color)
        {
            return (int)((color.A << 24) | (color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }
    }
}