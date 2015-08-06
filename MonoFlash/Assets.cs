using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoFlash
{
    class Assets
    {
        public static ContentManager content;

        public static BitmapData GetBitmapData(String path)
        {
            var bitmapData = new BitmapData();
            bitmapData.texture = content.Load<Texture2D>(path);
            return bitmapData;
        }
    }
}
