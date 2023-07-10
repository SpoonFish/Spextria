using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text;
using Microsoft.Xna.Framework;
using Spextria.Master;

namespace Spextria.Statics
{
    static class Functions
    {
        public static double Bearing(Vector2 v1, Vector2 v2)
        {
            double x = v2.X - v1.X;
            double y = v2.Y - v1.Y;

            double angle = (360 + Math.Atan2(x, y) * (180 / Math.PI)) % 360;
            // Math.Atan2 can return negative value, 0 <= output value < 2*PI expected 
            return angle;
        }
        public static double DegToRadians(double angle)
        {
            return angle* Math.PI / 180.0d;
        }
    }
}
