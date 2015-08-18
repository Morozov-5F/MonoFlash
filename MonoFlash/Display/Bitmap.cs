using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace MonoFlash.Display
{
    /// <summary>
    /// A basic unit which can be displayed on screen. 
    /// Cannot have children
    /// </summary>
    public class Bitmap : DisplayObject
    {
        private BitmapData bitmapData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bitmapData">BitmapData to be rendered</param>
        public Bitmap(BitmapData bitmapData)
        { 
            this.bitmapData = bitmapData;
            width = bitmapData.texture.Width;
            height = bitmapData.texture.Height;
        }

        public override void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            var newTransform = this.transformMatrix * transform;
            Vector2 pos, scale; 
            float rot;
            if (!DecomposeMatrix(ref newTransform, out pos, out rot, out scale))
            {
                Debug.WriteLine("Error decomposing matrix");
                return;
            }
            spriteBatch.Draw(bitmapData.texture, pos, null, Color.White,  rot, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            base.Render(spriteBatch, transform);
        }

        public override Vector4 GetBounds()
        {
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

