using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.PortableExecutable;
using Spextria.Maps.LevelObjectTypes;
using Spextria.Content.Maps.LevelObjectsTypes;
using Spextria.Graphics.GUI;
using Microsoft.Xna.Framework;
using Spextria.Entities;
using Spextria.Entities.Variants;
using Spextria.Statics;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using System.Net.WebSockets;

namespace Spextria.Maps
{
    static class MapLoader
    {
        static private Dictionary<string,string> PropertyCopy(Dictionary<string,string> properties)
        {
            Dictionary<string, string> newProperties = new Dictionary<string, string>();

            foreach (string key in properties.Keys)
            {
                newProperties.Add(key, properties[key]);
            }

            return newProperties;
        }
        static public LoadedMapObjects LoadMap(string mapString, EntityManager entityManager)
        {
            List<LoaderObject> levelObjects = new List<LoaderObject>();
            List<Rectangle> collisions = new List<Rectangle>();
            List<Rectangle> semiSolids = new List<Rectangle>();
            List<GroundType> groundTypes = new List<GroundType>();
            List<LoaderObject> collectables = new List<LoaderObject>();
            List<TextBox> textBoxes= new List<TextBox>();
            List<LoaderObject> enemies = new List<LoaderObject>();

            Spawnpoint spawnpoint = new Spawnpoint(new Vector2(0,0));
            Checkpoint checkpoint = new Checkpoint(new Vector2(0,0), new Vector2(-64, -64), new Rectangle(-64,-64,64,64), "luxiar1");
            Goalpoint goalpoint = new Goalpoint(new Vector2(-64,-64), new Rectangle(-64, -64, 64, 64), "luxiar1", "walk");

            Dictionary<string, string> properties = new Dictionary<string,string> { };
            string objectName;
            int x;
            int y;
            int width;
            int height;
            int mapWidth;
            int mapHeight;

            // groupReaderSettings settings = new groupReaderSettings();
            //settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader mapReader = XmlReader.Create(new StringReader(mapString));

            mapReader.ReadToFollowing("map");
            mapReader.MoveToAttribute(4);
            mapWidth = int.Parse(mapReader.Value);
            mapReader.MoveToAttribute(5);
            mapHeight = int.Parse(mapReader.Value);

            mapReader.ReadToFollowing("objectgroup");
            XmlReader groupReader = mapReader.ReadSubtree();

            //groupReader.MoveToAttribute(2);
            //groupName = groupReader.Value;

            groupReader.ReadToFollowing("object");
            do
            {
                groupReader.MoveToAttribute(1);
                string type = groupReader.Value;
                groupReader.MoveToAttribute(2);
                x = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(3);
                y = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(4);
                width = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(5);
                height = (int)float.Parse(groupReader.Value);

                
                groundTypes.Add(new GroundType(new Rectangle(x,y,width,height),type));
                properties.Clear();
            } while (groupReader.ReadToFollowing("object"));

            mapReader.ReadToFollowing("objectgroup");
            groupReader = mapReader.ReadSubtree();

            groupReader.ReadToFollowing("object");
            do
            {
                groupReader.MoveToAttribute(1);
                objectName = groupReader.Value;

                switch (objectName)
                {
                    case "spawnpoint":
                        {
                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(2);
                            x = int.Parse(groupReader.Value);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(2);
                            y = int.Parse(groupReader.Value);

                            spawnpoint = new Spawnpoint(new Vector2(x, y));
                            break;
                        }
                    case "checkpoint":
                        {
                            groupReader.MoveToAttribute(1);
                            objectName = groupReader.Value;
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);
                            Vector2 pos = new Vector2(x, y);
                            Rectangle area = new Rectangle(x, y, width, height);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(2);
                            x = int.Parse(groupReader.Value);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(2);
                            y = int.Parse(groupReader.Value);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            string type = groupReader.Value;

                            checkpoint = new Checkpoint(new Vector2(x, y), pos, area, type);
                            break;
                        }
                    case "finish":
                        {
                            groupReader.MoveToAttribute(1);
                            objectName = groupReader.Value;
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);
                            Vector2 pos = new Vector2(x, y);
                            Rectangle area = new Rectangle(x, y, width, height);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            string action = groupReader.Value;

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            string type = groupReader.Value;

                            goalpoint = new Goalpoint(pos, area, type, action);
                            break;
                        }
                    case "text":
                        {

                            groupReader.MoveToAttribute(1);
                            objectName = groupReader.Value;
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            Vector2 pos = new Vector2(x, y);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            string text = groupReader.Value;

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            string type = groupReader.Value;

                            textBoxes.Add(new TextBox(text, pos, width, type));
                            properties.Clear();
                            break;
                        }
                    case "enemy":
                        {

                            groupReader.MoveToAttribute(1);
                            objectName = groupReader.Value;
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("type", groupReader.Value);

                            enemies.Add(new LoaderObject(new Rectangle(x, y, 0, 0), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "damage_area":
                        {

                            groupReader.MoveToAttribute(1);
                            properties.Add("name", groupReader.Value);
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);
                            Vector2 pos = new Vector2(x, y);
                            Rectangle area = new Rectangle(x, y, width, height);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(2);
                            properties.Add("damage", groupReader.Value);
                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(2);
                            properties.Add("knockback", groupReader.Value);

                            levelObjects.Add(new LoaderObject(new Rectangle(x, y, width, height), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "gate":
                        {

                            groupReader.MoveToAttribute(1);
                            properties.Add("name", groupReader.Value);
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);
                            Vector2 pos = new Vector2(x, y);
                            Rectangle area = new Rectangle(x, y, width, height);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("image", groupReader.Value);
                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("state", groupReader.Value);
                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("trigger", groupReader.Value);

                            levelObjects.Add(new LoaderObject(new Rectangle(x, y, width, height), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "semisolid":
                        {

                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);


                            semiSolids.Add(new Rectangle(x, y, width, height));
                            properties.Clear();
                            break;
                        }
                    case "animated_image":
                        {

                            groupReader.MoveToAttribute(1);
                            properties.Add("name", groupReader.Value);
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("image", groupReader.Value);


                            levelObjects.Add(new LoaderObject(new Rectangle(x,y,width,height), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "npc":
                        {

                            groupReader.MoveToAttribute(1);
                            properties.Add("name", groupReader.Value);
                            groupReader.MoveToAttribute(2);
                            x = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(3);
                            y = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(4);
                            width = (int)float.Parse(groupReader.Value);
                            groupReader.MoveToAttribute(5);
                            height = (int)float.Parse(groupReader.Value);

                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("npc", groupReader.Value);


                            levelObjects.Add(new LoaderObject(new Rectangle(x, y, width, height), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }

                }

            } while (groupReader.ReadToFollowing("object"));


            mapReader.ReadToFollowing("objectgroup");
            groupReader = mapReader.ReadSubtree();

            groupReader.ReadToFollowing("object");
            do
            {
                groupReader.MoveToAttribute(1);
                objectName = groupReader.Value;
                properties.Add("name", groupReader.Value);
                groupReader.MoveToAttribute(2);
                x = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(3);
                y = (int)float.Parse(groupReader.Value);


                switch (objectName)
                {
                    case "big_coin":
                        {
                            collectables.Add(new LoaderObject(new Rectangle(x, y, 0, 0), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "coin":
                        {
                            collectables.Add(new LoaderObject(new Rectangle(x,y,0,0), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "healthpack":
                        {
                            collectables.Add(new LoaderObject(new Rectangle(x, y, 0, 0), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                    case "item":
                        {
                            groupReader.ReadToFollowing("property");
                            groupReader.MoveToAttribute(1);
                            properties.Add("type", groupReader.Value);
                            collectables.Add(new LoaderObject(new Rectangle(x, y, 0, 0), PropertyCopy(properties)));
                            properties.Clear();
                            break;
                        }
                }
            } while (groupReader.ReadToFollowing("object"));


            mapReader.ReadToFollowing("objectgroup");

            groupReader = mapReader.ReadSubtree();

            groupReader.ReadToFollowing("object");
            do
            {
                groupReader.MoveToAttribute(1);
                x = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(2);
                y = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(3);
                width = (int)float.Parse(groupReader.Value);
                groupReader.MoveToAttribute(4);
                height = (int)float.Parse(groupReader.Value);
                Rectangle area = new Rectangle(x, y, width, height);

                collisions.Add(area);

            } while (groupReader.ReadToFollowing("object"));

            Rectangle[] collisonArray = collisions.ToArray();
            Rectangle[] semiSolidArray = semiSolids.ToArray();
            LoadedMapObjects objects = new LoadedMapObjects(mapWidth, mapHeight, collisonArray, collectables, textBoxes, groundTypes, spawnpoint, checkpoint, goalpoint, levelObjects, enemies, semiSolidArray);
            return objects;
        }
    
    }
}
