using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFlash.Display
{
    class Bitmap : DisplayObject
    {
        private BitmapData bitmapData;
        public Color colorMask;

        public Bitmap(BitmapData bitmapData)
        {
            if (bitmapData == null)
                throw new ArgumentNullException();

            this.bitmapData = bitmapData;
            this.width = bitmapData.texture.Width;
            this.height = bitmapData.texture.Height;

            colorMask = Color.White;
            scale = Vector2.One;
        }

        public override void render(SpriteBatch spriteBatch)
        {
            if (!isVisible)
            {
                return;
            }
            Vector2 pos, scale;
            float rot;

            decomposeMatrix(ref transformMatrix, out pos, out rot, out scale);
            spriteBatch.Draw(bitmapData.texture, pos, null, colorMask, rot, Vector2.Zero, scale, SpriteEffects.None, 0);

            base.render(spriteBatch);
        }
    }
}
