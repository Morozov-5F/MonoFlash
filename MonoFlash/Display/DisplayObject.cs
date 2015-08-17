using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using DeathAboveUs.MonoFlash.Events;

namespace DeathAboveUs.MonoFlash.Display
{
    public class DisplayObject : EventDispatcher
    {
        public static Matrix TRANSFORM_ABSOLUTE = Matrix.Identity;
        protected float width, height;
        protected Matrix transformMatrix;
        protected Matrix globalTransform;
        public float layerDepth;
        public Stage stage;

        public float X
        { 
            get
            {
                return transformMatrix.Translation.X;
            }
            set
            {
                transformMatrix = Matrix.CreateScale(new Vector3(ScaleX, ScaleY, 1)) * 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) * 
                    Matrix.CreateTranslation(value, Y, 0);
            }
        }
        public float Y
        {
            get
            {
                return transformMatrix.Translation.Y;
            }
            set
            {
                transformMatrix = Matrix.CreateScale(new Vector3(ScaleX, ScaleY, 1)) * 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) * 
                    Matrix.CreateTranslation(X, value, 0);
            }   
        }
        public float Rotation
        {
            get
            {
                Vector2 pos = Vector2.Zero, scale = Vector2.Zero;
                float rotation;
                if (!DecomposeMatrix(ref transformMatrix, out pos, out rotation, out scale))
                {
                    return 0;
                }
                return MathHelper.ToDegrees(rotation);
            }
            set
            {
                transformMatrix = Matrix.CreateScale(new Vector3(ScaleX, ScaleY, 1)) * 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(value)) * 
                    Matrix.CreateTranslation(X, Y, 0);
            }
        }
        public float ScaleX
        {
            get
            {
                Vector3 pos = Vector3.Zero, scale = Vector3.Zero;
                Quaternion rotation;
                if (!transformMatrix.Decompose(out scale, out rotation, out pos))
                {
                    return 0;
                }
                return scale.X;
            }
            set
            {
                transformMatrix = Matrix.CreateScale(new Vector3(value, ScaleY, 1)) * 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) * 
                    Matrix.CreateTranslation(X, Y, 0);
            }
        }
        public float ScaleY
        {
            get
            {
                Vector3 pos = Vector3.Zero, scale = Vector3.Zero;
                Quaternion rotation;
                if (!transformMatrix.Decompose(out scale, out rotation, out pos))
                {
                    return 0;
                }
                return scale.Y;
            }
            set
            {
                transformMatrix = Matrix.CreateScale(new Vector3(ScaleX, value, 1)) * 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) * 
                    Matrix.CreateTranslation(X, Y, 0);
            }
        }
        public float Width
        {
            get
            {
                Vector4 bounds = GetBounds();
                return Math.Abs(bounds.X - bounds.Z);
            }
            set
            {
                width = value;
            }
        }        
        public float Height
        {
            get
            {
                Vector4 bounds = GetBounds();
                return Math.Abs(bounds.Y - bounds.W);
            }
            set
            {
                height = value;
            }
        }
        public DisplayObject parent;

        public DisplayObject()
        {
            transformMatrix = Matrix.CreateScale(new Vector3(1, 1, 1)) * 
                        Matrix.CreateRotationZ(0) * 
                        Matrix.CreateTranslation(0, 0, 0);
            width = 0; height = 0;
            parent = null;
            
            AddEventListener(Event.ADDED_TO_STAGE, AddedToStage);
        }

        private void AddedToStage(Event e)
        {
            stage = parent.stage;
        }

        public virtual void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            globalTransform = transform;
            DispatchEvent(new Event(Event.ENTER_FRAME));
        }        

        public virtual bool HitTestPoint(Point point)
        {
            // Two variants: 1) pass always global point
            //               2) make transform on your own beforn calling HitTestPoint()
            var bounds = GetBounds();
            BoundingBox bb = new BoundingBox(new Vector3(bounds.X, bounds.Y, 0), 
                new Vector3(bounds.Z, bounds.W, 0));
            return (bb.Contains(new Vector3(point.X, point.Y, 0)) == ContainmentType.Contains);
        }

        public virtual bool HitTestObject(DisplayObject obj)
        {
            var bounds1 = GetBounds();
            var bounds2 = obj.GetBounds();
            // TODO: Coordinates should be all transformed to global!
            BoundingBox bb1 = new BoundingBox(new Vector3(bounds1.X, bounds1.Y, 0), 
                                              new Vector3(bounds1.Z, bounds1.W, 0)),
                        bb2 = new BoundingBox(new Vector3(bounds2.X, bounds2.Y, 0), 
                                              new Vector3(bounds2.Z, bounds2.W, 0));
            return false;
        }

        public virtual Vector2 GlobalToLocal(Vector2 point)
        {
            var pointMatrix = new Matrix(point.X, point.Y, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);
            pointMatrix *= Matrix.Invert(globalTransform);
            return new Vector2(pointMatrix.M11, pointMatrix.M12);
        }

        public virtual Vector2 LocalToGlobal(Vector2 point)
        {
            var pointMatrix = new Matrix(point.X, point.Y, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);
            pointMatrix *= globalTransform;
            return new Vector2(pointMatrix.M11, pointMatrix.M12);
        }

        public virtual Vector4 GetBounds()
        {
            return Vector4.Zero;
        }

        protected bool DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            position = Vector2.Zero; scale = Vector2.One; rotation = 0;

            if (!matrix.Decompose(out scale3, out rotationQ, out position3))
            {
                return false;
            }

            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
           
            return true;
        }
    }
}

