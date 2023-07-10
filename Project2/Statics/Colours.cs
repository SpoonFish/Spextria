using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Spextria.Statics
{
    static class Colours
    {
        public static Dictionary<string, Color> ColourDict = new Dictionary<string, Color>();
        
        public static void LoadColours()
        {
            ColourDict.Add("white", new Color(255, 255, 255));
            ColourDict.Add("yellow", new Color(255, 243, 94));
            ColourDict.Add("green", new Color(41, 218, 1));
            ColourDict.Add("purple", new Color(188, 0, 255));
            ColourDict.Add("blue", new Color(0, 94, 255));
            ColourDict.Add("orange", new Color(235, 159, 0));
            ColourDict.Add("red", new Color(235, 0, 31));
            ColourDict.Add("grey", new Color(128, 128, 128));
        }
    }
}
