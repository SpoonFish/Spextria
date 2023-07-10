using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Graphics.GUI.PlayScreen;
using Spextria.Master;
using Spextria.Statics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Entities
{
    class MonsterHealthBar
    {
        private Bar Bar;
        private MyTexture Underlay;
        public bool Active;
        public MonsterHealthBar(Bar bar)
        {
            Active = true;
            Bar = bar;
            Underlay = Images.ImageDict["enemy_bar_underlay"];
        }

        public void Update(Vector2 pos, MasterManager master, float value)
        {
            Bar.Position = pos;
            Bar.Update(value, master);


            if (!master.storedDataManager.CheckSkillUnlock("scanner"))
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                Underlay.Draw(spriteBatch, Bar.Position + new Vector2(-1, -1));
                Bar.Draw(spriteBatch);

            }
        }
    }
}
