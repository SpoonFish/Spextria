using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Content.Maps.LevelObjectsTypes
{
    class Spawnpoint
    {
        public Vector2 SpawnPosition;
        public Spawnpoint(Vector2 spawnPosition)
        {
            SpawnPosition = spawnPosition;
        }
    }
}
