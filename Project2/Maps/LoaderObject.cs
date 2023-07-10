using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spextria.Maps
{
    class LoaderObject
    {
        public Rectangle Rect;
        public Dictionary<string, string> Properties;

        public LoaderObject(Rectangle rect, Dictionary<string, string> properties)
        {
            Rect = rect;
            Properties = properties;
        }
    }
}
