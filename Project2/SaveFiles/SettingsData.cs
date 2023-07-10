using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Spextria.StoredData
{
    public class SettingsData
    {
        public bool AttackTowardsMouse { get; set; }
        public int MusicVolume { get; set; }
        public int SoundVolume { get; set; }

    }
}
