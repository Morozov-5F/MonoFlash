using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFlash.Display
{
    abstract class DisplayObject
    {
        public Vector2 pos, scale;
        public int width, height;
        public float rotation;
        public Sprite parent;

        protected Matrix transformMatrix;
        protected Matrix TransformMatrix
        {
            get
            {
                Matrix transform =
                    Matrix.CreateScale(new Vector3(scale, 1)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) *
                    Matrix.CreateTranslation(new Vector3(pos, 0));
                return transform;
            }
            set
            {
                transformMatrix = value;
            }
        }

        abstract public void Draw(SpriteBatch spriteBatch);
        public void ApplyTransformMatrix(Matrix parentMatrix)
        {
            TransformMatrix *= parentMatrix;
        }
        public static void DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }
    }
}
