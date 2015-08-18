using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Display;

namespace MonoFlash
{
    /// <summary>
    /// Class for handling assets. Basically, it's a wrap of standard MonoFame content
    /// </summary>
    class Assets
    {
        public static ContentManager content;

        /// <summary>
        /// Gets a bitmap data from path
        /// </summary>
        /// <param name="path">Path to texture (bitmapData)</param>
        /// <returns>BitmapData if succeeded</returns>
        public static BitmapData GetBitmapData(String path)
        {
            var bitmapData = new BitmapData();
            bitmapData.texture = content.Load<Texture2D>(path);
            return bitmapData;
        }
    }
}
