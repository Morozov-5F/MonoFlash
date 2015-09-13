using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Display;

using System.Text;
using System.Diagnostics;

namespace MonoFlash
{
    /// <summary>
    /// Class for handling assets. Basically, it's a wrap of standard MonoFame content
    /// </summary>
    public class Assets
    {
        public static ContentManager content;
        private static Dictionary <string, object> assetCache = new Dictionary<string, object>();
        /// <summary>
        /// Gets a bitmap data from path
        /// </summary>
        /// <param name="path">Path to texture (bitmapData)</param>
        /// <param name="useCache">Usage of cache flag</param>
        /// <returns>BitmapData if succeeded, null if not</returns>
        public static BitmapData GetBitmapData(String path, bool useCache = false)
        {
            var bitmapData = new BitmapData();
            try
            {
                if (useCache && assetCache.ContainsKey(path))
                {
                    bitmapData = (assetCache[path] as BitmapData);
                }
                else
                {
                    bitmapData.texture = content.Load<Texture2D>(path);
                    if (useCache)
                    {
                        assetCache.Add(path, bitmapData);
                    }
                }
            }
            catch (ContentLoadException exc)
            {
                Debug.WriteLine(exc);
                return null;
            }
            return bitmapData;
        }
        /// <summary>
        /// Gets the SpriteFont.
        /// </summary>
        /// <param name="path">Path to font</param>
        /// <returns>loaded Font</returns>
        public static SpriteFont GetFont(String path)
        {
            var font = content.Load<SpriteFont>(path);
            return font;
        }
    }
}
