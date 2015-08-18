using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    /// <summary>
    /// A basic unit that handles all the transforms and coordinates translation
    /// </summary>
    public class DisplayObject : EventDispatcher
    {
        public static Matrix TRANSFORM_ABSOLUTE = Matrix.Identity;
        /// <summary>
        /// Local width and height
        /// </summary>
        protected float width, height;
        /// <summary>
        /// Object's transform matrix
        /// </summary>
        protected Matrix transformMatrix;
        /// <summary>
        /// Parent's transform matrix
        /// </summary>
        protected Matrix globalTransform;
        /// <summary>
        /// Gets and sets globalTransform of an object
        /// </summary>
        public Matrix GlobalTransform
        {
            get 
            {
                return globalTransform;
            }
            set
            {
                globalTransform = value;
            }
        }   

        public float layerDepth;
        public Stage stage;

        /// <summary>
        /// X position, relative to parent
        /// </summary>
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
        /// <summary>
        /// Y position, relative to parent
        /// </summary>
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
        /// <summary>
        /// Rotation in degrees, relative to parent
        /// </summary>
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
        /// <summary>
        /// X scale, relative to parent
        /// </summary>
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
        /// <summary>
        /// Y scale, relative to parent
        /// </summary>
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
        /// <summary>
        /// Width of an object, without applying a parent transform
        /// </summary>
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
        /// <summary>
        /// Height of an object, without applying a parent transform
        /// </summary>
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
        /// <summary>
        /// Parent DisplayObject
        /// </summary>
        public DisplayObject parent;
        /// <summary>
        /// Constructor
        /// </summary>
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
        /// <summary>
        /// Renders an object at screen with given transform matrix
        /// </summary>
        /// <param name="spriteBatch">SpriteBathch to draw</param>
        /// <param name="transform">Transform matrix to apply</param>
        public virtual void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            globalTransform = transform;
            DispatchEvent(new Event(Event.ENTER_FRAME));
        }        
        /// <summary>
        /// Checks if object contains a point
        /// </summary>
        /// <param name="point">point in object's local coordinates</param>
        /// <returns>true, if object contains point, false otherwise</returns>
        public virtual bool HitTestPoint(Vector2 point)
        {
            // Two variants: 1) pass always global point
            //               2) make transform on your own beforn calling HitTestPoint()
            // Yet selected second variant
            var bounds = GetBounds();
            return (point.X >= bounds.X) && (point.X <= bounds.Z) && 
+                   (point.Y >= bounds.Y) && (point.Y <= bounds.W);
        }
        /// <summary>
        /// Checks if object intersects with other 
        /// NOT IMPLEMENTED YET
        /// </summary>
        /// <param name="obj">Displayobject to check intersection with</param>
        /// <returns>true, if objects intersects, false otherwise</returns>
        public virtual bool HitTestObject(DisplayObject obj)
        {
            throw new NotImplementedException("A HitTestObject is not implemented yet");

            var bounds1 = GetBounds();
            var bounds2 = obj.GetBounds();
            // TODO: Coordinates should be all transformed to global!
            BoundingBox bb1 = new BoundingBox(new Vector3(bounds1.X, bounds1.Y, 0), 
                                              new Vector3(bounds1.Z, bounds1.W, 0)),
                        bb2 = new BoundingBox(new Vector3(bounds2.X, bounds2.Y, 0), 
                                              new Vector3(bounds2.Z, bounds2.W, 0));
            return false;
        }
        /// <summary>
        /// Transforms global point to local coordinates
        /// </summary>
        /// <param name="point">Point to transform</param>
        /// <returns>Transformed point</returns>
        public virtual Vector2 GlobalToLocal(Vector2 point)
        {
            var pointMatrix = new Matrix(point.X, point.Y, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);
            pointMatrix *= Matrix.Invert(globalTransform);
            return new Vector2(pointMatrix.M11, pointMatrix.M12);
        }
        /// <summary>
        /// Transforms local point to global coordinates
        /// </summary>
        /// <param name="point">Point to transform</param>
        /// <returns>Transformed point</returns>
        public virtual Vector2 LocalToGlobal(Vector2 point)
        {
            var pointMatrix = new Matrix(point.X, point.Y, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1);
            pointMatrix *= globalTransform;
            return new Vector2(pointMatrix.M11, pointMatrix.M12);
        }
        /// <summary>
        /// Gets bounds of a DisplayObject.
        /// </summary>
        /// <returns>Vector4, generated in that way: (X:top-leftX, Y:top-leftY, Z:bot-rightX, W:bot-rightY)</returns>
        public virtual Vector4 GetBounds()
        {
            return Vector4.Zero;
        }
        /// <summary>
        /// Decomposes transform matrix to position, rotation and scale
        /// </summary>
        /// <param name="matrix">Matrix to decompose</param>
        /// <param name="position">Resulting position</param>
        /// <param name="rotation">Resulting rotation in radians</param>
        /// <param name="scale">Resulting scale</param>
        /// <returns>true if decomposition is possible, false otherwise</returns>
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

