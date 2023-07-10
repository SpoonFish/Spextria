using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Spextria.Statics
{
    static class LevelInfo
    {
        public static Dictionary<string, string> LevelNames = new Dictionary<string, string>();
        
        public static void LoadLevelInfo()
        {
            LevelNames.Add("1", "TRAINING GROUNDS");
            LevelNames.Add("2", "DEAD-THORN DUNES");
            LevelNames.Add("3", "BLOODSCALE'S CAMP");
            LevelNames.Add("4", "SANDWORM PLATEAU");
            LevelNames.Add("5", "DEATHSCALE'S OUTPOST");
        }
    }
}
