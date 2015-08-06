using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    abstract class DisplayObject : EventDispatcher
    {
        public Vector2 pos, scale;
        public float X          { get { return pos.X; }     set { pos.X = value; } }
        public float Y          { get { return pos.Y; }     set { pos.Y = value; } }
        public float ScaleX     { get { return scale.X; }   set { scale.X = value; } }
        public float ScaleY     { get { return scale.Y; }   set { scale.Y = value; } }
        public int width, height;
        public float rotation   = 0f;
        public float alpha      = 1f;
        public bool isVisible   = true;

        public Stage stage;
        public Sprite parent;

        protected float absoluteAlpha = 1f;

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
            width = 0;
            height = 0;

            AddEventListener(Event.ADDED_TO_STAGE, AddedToStage);
        }

        private void AddedToStage(Event e)
        {
            stage = parent.stage;
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            if (!isVisible)
            {
                return;
            }
            absoluteAlpha = alpha * parent.alpha;
            DispatchEvent(new Event(Event.ENTER_FRAME));
        }
        
        public abstract Vector4 GetBounds();
        
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
