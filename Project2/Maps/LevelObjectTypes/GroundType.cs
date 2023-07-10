using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Maps.LevelObjectTypes
{
    class GroundType
    {
        public Rectangle Area;
        private string Type;
        public string ParticleType;
        public GroundType(Rectangle area, string type)
        {
            Area = area;
            Type = type;
            switch (type)
            {
                case "sand":
                    ParticleType = "yellow_dust";
                    break;
                case "dirt":
                    ParticleType = "white_dust";
                    break;
                case "stone":
                    ParticleType = "white_flecks";
                    break;
                case "metal":
                    ParticleType = "sparks";
                    break;
            }
        }
    }
}
