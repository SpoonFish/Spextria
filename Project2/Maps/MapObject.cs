using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Spextria.Graphics;
using Spextria.Content.Maps.LevelObjectsTypes;
using Spextria.Maps.LevelObjectTypes;
using Spextria.Master;
using Spextria.Graphics.GUI;
using Spextria.Statics;
using Spextria.Entities.Variants;
using MonoGame.Extended.Sprites;
using System.Diagnostics.Metrics;
using Microsoft.Xna.Framework.Input;

namespace Spextria.Maps
{
    class MapObject
    {
        private Texture2D MapImage;
        private int Width;
        private int Height;
        public int Bottom;
        public List<LoaderObject> Collectables;
        public Rectangle[] Collisions;
        public Rectangle[] Semisolids;
        public List<TextBox> TextBoxes;
        public List<GroundType> Grounds;
        public List<Gate> Gates;
        public List<LevelObject> LevelObjects;
        public List<LoaderObject> LevelObjectsUnloaded;
        public List<LoaderObject> Enemies;
        private Spawnpoint LevelSpawnpoint;
        private Checkpoint LevelCheckpoint;
        private Goalpoint LevelGoal;
        public Dictionary<string, int> Triggers;

        public MapObject(Texture2D map, LoadedMapObjects objects)
        {
            Triggers = new Dictionary<string, int>() { };
            Triggers.Add("1", 0);
            Triggers.Add("2", 0);
            Triggers.Add("3", 0);
            Triggers.Add("4", 0);
            Triggers.Add("5", 0);

            MapImage = map;
            Width = map.Width;
            Height = map.Height;
            Bottom = map.Height * 16;

            Gates = new List<Gate>();
            Collectables = objects.Collectables;
            TextBoxes = objects.TextBoxes;
            LevelObjectsUnloaded = objects.LevelObjects;
            LevelObjects = new List<LevelObject>();
            Grounds = objects.Grounds;
            Enemies = objects.Enemies;
            Collisions = objects.Collisions;
            Semisolids = objects.Semisolids;

            LevelSpawnpoint = objects.LevelSpawnpoint;
            LevelGoal = objects.LevelGoal;
            LevelCheckpoint = objects.LevelCheckpoint;
        }
        
        public void LoadLayers(EntityManager entityManager)
        {
            Triggers = new Dictionary<string, int>() { };
            Triggers.Add("1", 0);
            Triggers.Add("2", 0);
            Triggers.Add("3", 0);
            Triggers.Add("4", 0);
            Triggers.Add("5", 0);
            //TiledMapObjectLayer collisionLayer = _map.GetLayer<TiledMapObjectLayer>("Collisions");
            //TiledMapObjectLayer levelObjectsLayer = _map.GetLayer<TiledMapObjectLayer>("LevelObjects");
            //TiledMapObjectLayer groundTypesLayer = _map.GetLayer<TiledMapObjectLayer>("GroundTypes");
            LevelGoal.Reset();
            entityManager.TextBoxArray = TextBoxes;
            entityManager.EnemiesArray.Clear();
            entityManager.CollectablesArray.Clear();
            LevelObjects.Clear();
            foreach (LoaderObject levelObject in LevelObjectsUnloaded)
            {
                switch (levelObject.Properties["name"])
                {
                    case "gate":
                        Gates.Add(new Gate(levelObject.Rect, levelObject.Properties["trigger"], levelObject.Properties["state"], Images.ImageDict[levelObject.Properties["image"]]));
                        continue;
                    case "damage_area":
                        LevelObjects.Add(new DamageArea(new Vector2(levelObject.Rect.X, levelObject.Rect.Y), levelObject.Rect, float.Parse(levelObject.Properties["damage"]), int.Parse(levelObject.Properties["knockback"])));
                        continue;
                    case "animated_image":
                        LevelObjects.Add(new AnimatedObject(new Vector2(levelObject.Rect.X, levelObject.Rect.Y), levelObject.Rect, Images.ImageDict[levelObject.Properties["image"]]));
                        continue;
                    case "npc":
                        LevelObjects.Add(new Npc(new Vector2(levelObject.Rect.X, levelObject.Rect.Y), levelObject.Rect, levelObject.Properties["npc"]));
                        continue;
                    default:
                        continue;
                }

            }
            foreach (LoaderObject enemy in Enemies)
            {
                entityManager.EnemiesArray.Add(Monsters.CreateMonster(enemy.Properties["type"], new Vector2(enemy.Rect.X, enemy.Rect.Y)));
            }

            foreach (LoaderObject collectable in Collectables)
            {
                switch (collectable.Properties["name"])
                {
                    case "big_coin":
                        entityManager.CollectablesArray.Add(new BigCoin(Images.ImageDict["big_coin"], new Vector2(collectable.Rect.X, collectable.Rect.Y), 2, 1));
                        continue;
                    case "coin":
                        entityManager.CollectablesArray.Add(new Coin(Images.ImageDict["coin"], new Vector2(collectable.Rect.X, collectable.Rect.Y), 2, 1));
                        continue;
                    case "healthpack":
                        entityManager.CollectablesArray.Add(new RepairCell(Images.ImageDict["repair_cell"], new Vector2(collectable.Rect.X, collectable.Rect.Y), 2, false, 1));
                        continue;
                    case "item":
                        entityManager.CollectablesArray.Add(new UnlockableItem(Images.ImageDict[collectable.Properties["type"]], new Vector2(collectable.Rect.X, collectable.Rect.Y), 2, 1));
                        continue;
                }
            }
        }
            
        public bool LevelCompleted()
        {
            return LevelGoal.Reached;
        }
        public Vector2 GetCheckpoint()
        {
            return LevelCheckpoint.SpawnPosition;
        }
        public Vector2 GetSpawnpoint()
        {
            return LevelSpawnpoint.SpawnPosition;
        }
        public Texture2D GetMap()
        {
            return MapImage;
        }

        public int GetWidth()
        {
            return Width;
        }

        public int GetHeight()
        {
            return Height;
        }

        public void UpdateObjects(MasterManager master)
        {
            LevelCheckpoint.Update(master);
            LevelGoal.Update(master);
            foreach (LevelObject levelObject in LevelObjects)
                levelObject.Update(master);
            foreach (Gate gate in Gates)
                gate.Update(master);
        }
        public void DrawObjects(SpriteBatch spriteBatch)
        {
            LevelCheckpoint.Draw(spriteBatch);
            LevelGoal.Draw(spriteBatch);
            foreach (LevelObject levelObject in LevelObjects)
                levelObject.Draw(spriteBatch);
            foreach (Gate gate in Gates)
                gate.Draw(spriteBatch);
        }
    }
}
