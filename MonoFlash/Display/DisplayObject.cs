using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    abstract class DisplayObject : EventDispatcher
    {
        public Vector2 pos, scale;
        public float x { get { return pos.X; } set { pos.X = value; } }
        public float y { get { return pos.Y; } set { pos.Y = value; } }
        public float scaleX { get { return scale.X; } set { scale.X = value; } }
        public float scaleY { get { return scale.Y; } set { scale.Y = value; } }
        public float width, height;
        public float rotation   = 0f;
        //public float alpha      = 1f;
        public bool isVisible   = true;

        public Stage stage;
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


        public DisplayObject()
        {
            pos = Vector2.Zero;
            scale = Vector2.One;
            width = 0f;
            height = 0f;

            addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
        }

        private void onAddedToStage(Event e)
        {
            stage = parent.stage;
        }

        public virtual void render(SpriteBatch spriteBatch)
        {
            if (!isVisible)
            {
                return;
            }
            dispatchEvent(new Event(Event.ENTER_FRAME));
        }

        public void applyTransformMatrix(Matrix parentMatrix)
        {
            TransformMatrix *= parentMatrix;
        }

        public static void decomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
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
