using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MonoFlash.Display
{
    public class Bitmap : DisplayObject
    {
        protected BitmapData bitmapData;
        public Color color;

        public Bitmap()
        {
            width = 0;
            height = 0;
            color = Color.White;
        }

        public Bitmap(BitmapData bitmapData)
        { 
            if (bitmapData == null)
                throw new ArgumentNullException("Given BitmapData is null");
            
            this.bitmapData = bitmapData;
            width = bitmapData.texture.Width;
            height = bitmapData.texture.Height;
            color = Color.White;
        }

        public override void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            if (bitmapData == null)
            {
                return;
            }
            var newTransform = this.transformMatrix * transform;
            Vector2 pos, scale; 
            float rot;
            if (!DecomposeMatrix(ref newTransform, out pos, out rot, out scale))
            {
                Debug.WriteLine("Error decomposing matrix");
                return;
            }
            SpriteEffects effects = (!flippedX) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            effects |= (!flippedY) ? SpriteEffects.None : SpriteEffects.FlipVertically;

            spriteBatch.Draw(bitmapData.texture, pos, null, color,  rot, Vector2.Zero, scale, effects, layerDepth);
            base.Render(spriteBatch, transform);
        }

        public override Vector4 GetBounds()
        {
            if (bitmapData == null)
            {
                return new Vector4();
            }

            var matrix = new Matrix(0, 0, 0, 1, width, 0, 0, 1, 0, height, 0, 1, width, height, 0, 1);
            matrix *= transformMatrix;
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
            return new Vector4(minX, minY, maxX, maxY);
        }
    }
}

