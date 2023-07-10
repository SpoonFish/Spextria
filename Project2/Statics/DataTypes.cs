using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spextria.Statics
{
    static class DataTypes
    {
        public enum Directions
        {
            Left,
            Right,
            Down,
            Up,
            None
        }
        public enum Hostility
        {
            Passive,
            NeutralForgivable,
            Neutral,
            Hostile,
        }
        public enum Intelligence
        {
            Stationary,
            Wander,
            Scout,
            Leap,
            Sidetoside,
            Approach,
            Worm1,
        }
    }
}
