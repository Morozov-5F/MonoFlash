using System;
using MonoFlash.Display;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoFlash.Text
{
    public class TextField : DisplayObject
    {
        public SpriteFont font;
        public string text;
        public Color textColor;


        public TextField()
        {
            textColor = Color.White;
        }

        public override void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            var newTransform = this.transformMatrix * transform;
            Vector2 pos, scale; 
            float rot;
            DecomposeMatrix(ref newTransform, out pos, out rot, out scale);
            spriteBatch.DrawString(font, text, pos, textColor, rot, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            base.Render(spriteBatch, transform);
        }

        public override Vector4 GetBounds()
        {
            var textBounds = font.MeasureString(text);

            var matrix = new Matrix(0, 0, 0, 1, textBounds.X, 0, 0, 1, 0, textBounds.Y, 0, 1, textBounds.X, textBounds.Y, 0, 1);
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

