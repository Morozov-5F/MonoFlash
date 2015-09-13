using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Display;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MonoFlash
{
    public class Assets
    {
        public static ContentManager content;
        private static Dictionary <string, object> assetCache = new Dictionary<string, object>();

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

        public static SpriteFont GetFont(String path)
        {
            var font = content.Load<SpriteFont>(path);
            return font;
        }
    }
}
