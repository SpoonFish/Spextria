using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using Spextria.Content.Maps.LevelObjectsTypes;
using Spextria.Entities;
using Spextria.Graphics.GUI;
using Spextria.Maps.LevelObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spextria.Maps
{
    class LoadedMapObjects
    {
        public int Width;
        public int Height;
        public List<LoaderObject> Collectables;
        public Rectangle[] Collisions;
        public Rectangle[] Semisolids;
        public List<TextBox> TextBoxes;
        public List<GroundType> Grounds;
        public List<LoaderObject> LevelObjects;
        public List<LoaderObject> Enemies;
        public Spawnpoint LevelSpawnpoint;
        public Checkpoint LevelCheckpoint;
        public Goalpoint LevelGoal;

        public LoadedMapObjects(int width, int height, Rectangle[] collisions, List<LoaderObject> collectables, List<TextBox> textBoxes, List<GroundType> grounds, Spawnpoint spawnpoint, Checkpoint checkpoint, Goalpoint goalpoint, List<LoaderObject> levelObjects, List<LoaderObject> enemies, Rectangle[] semiSolids)
        {
            Semisolids = semiSolids;
            Width = width;
            Height = height;
            Collisions = collisions;
            Enemies = enemies;
            Collectables = collectables;
            TextBoxes = textBoxes;
            Grounds = grounds;
            LevelObjects = levelObjects;
            LevelSpawnpoint = spawnpoint;
            LevelCheckpoint = checkpoint;
            LevelGoal = goalpoint;
            
        }
    }
}
