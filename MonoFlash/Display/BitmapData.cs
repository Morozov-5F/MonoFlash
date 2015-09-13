using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoFlash.Display
{
    public class BitmapData
    {
        public Texture2D texture;
        public Rectangle sourceRectangle;

        public BitmapData()
        {
        }

        public BitmapData(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
