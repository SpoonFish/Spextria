using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spextria.StoredData
{
    public class SaveFile
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public int PlanetsUnlocked { get; set; } //luxiar, gramen, freone, umbrac, inferni, ossium ::: malum
        public int CurrentLevel { get; set; }
        public int Coins { get; set; }
        public int LightSouls { get; set; }
        public int GrowthSouls { get; set; }
        public int FrostSouls { get; set; }
        public int ShadowSouls { get; set; }
        public int FlameSouls { get; set; }
        public int TormentSouls { get; set; }
        public int Checkpoint { get; set; }
        public bool ShowIntro { get; set; }
        public bool New { get; set; }

        public List<string> Cutscenes { get; set; }
        public List<string> Purchases { get; set; }
    }
}

