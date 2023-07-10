using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Graphics;
using Spextria.Graphics.GUI;
using Spextria.Graphics.GUI.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled.Renderers;

namespace Spextria.Statics
{
    static class Effects
    {
        public static Effect Greyscale;
        public static TiledMapEffect MapGreyscale;

        public static void LoadEffects(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Greyscale = (content.Load<Effect>("Effects/greyscale"));
            MapGreyscale = new TiledMapEffect(content.Load<Effect>("Effects/map_greyscale"));
        }
    }
}