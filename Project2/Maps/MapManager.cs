using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Spextria.Entities;
using Spextria.Statics;
using Spextria.Graphics;
using Spextria.Maps.LevelObjectTypes;
using Spextria.Master;

namespace Spextria.Maps
{
    class MapManager
    {
        public bool CurrentMapDrawn;
        private Dictionary<string, MapObject> MapDict;
        public MapObject CurrentMap;

        public MapManager(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphicsDevice, EntityManager spriteManager)
        {
            MapDict = new Dictionary<string, MapObject>();
            PreloadMaps(content, graphicsDevice, spriteManager);
            CurrentMap = MapDict["1"];
            //CurrentRenderer = RendererDict["1"];
            //Console.WriteLine(Map.Collision.Objects);
        }

        private void PreloadMaps(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphicsDevice, EntityManager spriteManager)
        {
            string mapData =
@"
1
2
3
4
";
            string[] mapDataList = mapData.Split("\r\n");
            foreach (string mapString in mapDataList)
            {
                if (mapString == "")
                    continue;
                Texture2D map = content.Load<Texture2D>("Maps/"+mapString);
                LoadedMapObjects objects = MapLoader.LoadMap(Statics.Maps.MapStrings[int.Parse(mapString)-1], spriteManager);
                MapDict.Add(mapString.Split("/")[^1].Replace("\n", ""), new MapObject(map, objects));
            }

        }
        public void LoadMap(string mapName, MasterManager master, GraphicsDevice graphicsDevice)
        {
            
            CurrentMap = MapDict[mapName];
            CurrentMap.LoadLayers(master.entityManager);
           
            master.MapTarget = new RenderTarget2D(graphicsDevice, CurrentMap.GetWidth(), CurrentMap.GetHeight());
            CurrentMapDrawn = false;
        }

        public GroundType GetGroundType(Rectangle hitbox)
        {
            foreach (GroundType ground in CurrentMap.Grounds)
            {
                if (ground.Area.Contains(hitbox.Center))
                    return ground;
            }
            return new GroundType(new Rectangle(), null);
        }
    }
}
