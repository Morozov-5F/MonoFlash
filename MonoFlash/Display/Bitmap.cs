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
        
        public override Vector4 GetBounds()
        {
            var rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);

            var matrix = new Matrix(new Vector4(rect.X, rect.Y, 0, 0), new Vector4(rect.X + rect.Width, rect.Y, 0, 0),
                                    new Vector4(rect.X, rect.Y + rect.Height, 0, 0), new Vector4(rect.X + rect.Width, rect.Y + rect.Height, 0, 0));
            matrix *= Matrix.CreateScale(new Vector3(scale, 1)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) ;
            // OH SHI~
            float minX = matrix[0, 0], maxX = matrix[0, 0], minY = matrix[0, 1], maxY = matrix[0, 1];
            for (int i = 1; i < 4; ++i)
            {
                var x = matrix[i, 0];
                minX = Math.Min(x, minX);
                maxX = Math.Max(x, maxX);
                var y = matrix[i, 1];
                minY = Math.Min(y, minY);
                maxY = Math.Max(y, maxY);
            }
            //rect = new Rectangle((int)topLeft.Translation.X, (int)topLeft.Translation.Y, (int)(topRight.Translation.X - topLeft.Translation.X), (int)(botLeft.Translation.Y - topLeft.Translation.Y));
            width = (int)Math.Ceiling(maxX - minX);
            height = (int)Math.Ceiling(maxY - minY);
            return new Vector4(minX, minY, maxX, maxY);
        }
    }
}
