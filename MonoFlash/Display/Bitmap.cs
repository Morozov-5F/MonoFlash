using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFlash.Display
{
    class Bitmap : DisplayObject
    {
        private Texture2D bitmapData;
        public Color colorMask;

        public Bitmap(Texture2D bitmapData)
        {
            if (bitmapData == null)
                throw new ArgumentNullException();

            this.bitmapData = bitmapData;
            this.width = bitmapData.Width;
            this.height = bitmapData.Height;

            colorMask = Color.White;
            scale = Vector2.One;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DisplayObject obj = this.parent;
            while (obj.parent != null)
            {
                obj = obj.parent;
            }
            Vector2 pos, scale;
            float rot;

            DecomposeMatrix(ref transformMatrix, out pos, out rot, out scale);
            spriteBatch.Draw(bitmapData, pos, null, colorMask, rot, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
