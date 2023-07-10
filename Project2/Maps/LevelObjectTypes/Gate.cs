using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Master;
using Spextria.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spextria.Maps.LevelObjectTypes
{
    class Gate
    {
        public Rectangle Rect;
        public string Trigger;
        public string State;
        public string StateWhenNotTriggered;
        public MyTexture Texture;
        public Vector2 Position;

        public Gate(Rectangle rect, string trigger, string state, MyTexture texture)
        {
            Position = new Vector2(rect.X, rect.Y);
            Texture = texture;
            Rect = rect;
            Trigger = trigger;
            State = state;
            StateWhenNotTriggered = state;
        }
        public void Update(MasterManager master)
        {
            if (master.mapManager.CurrentMap.Triggers[Trigger] == 0)
            {
                if (StateWhenNotTriggered == "on")
                    State = "on";
                else
                    State = "off";
            }
            else
            {
                if (StateWhenNotTriggered == "on")
                    State = "off";
                else
                    State = "on";
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == "on")
                Texture.Draw(spriteBatch, new Vector2(Rect.X, Rect.Y), 1, new Vector2(Rect.Width, Rect.Height));
        }
    }
}
