using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    /// <summary>
    /// Main purpose of this class is to be a container for other Sprites
    /// or Bitmaps. It can have children and can handle them
    /// </summary>
    public class Sprite : DisplayObject
    {
        /// <summary>
        /// Children list
        /// </summary>
        public List<DisplayObject> children;

        public Sprite()
        {
            children = new List<DisplayObject>();
        }
        /// <summary>
        /// Attaches a child to sprite
        /// </summary>
        /// <param name="obj">DisplayObject to attach</param>
        /// <returns>true, if child can be attached, flase otherwise</returns>
        public bool AddChild(DisplayObject obj)
        {
            if (obj.parent != null)
            {
                return false;
            }
            children.Add(obj);

            obj.GlobalTransform = transformMatrix;
            obj.parent = this;
            obj.DispatchEvent(new Event(Event.ADDED_TO_STAGE));
            return true;
        }
        /// <summary>
        /// Removes a child from sprite
        /// </summary>
        /// <param name="obj">DisplayObject to remove from children</param>
        /// <returns>true, if child is detached successfully, false otherwise</returns>
        public bool RemoveChild(DisplayObject obj)
        {
            if (!children.Remove(obj))
            {
                return false;
            }
            obj.parent = null;
            obj.stage = null;
            return true;
        }

        public override void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            var newTransform = this.transformMatrix * transform;
            foreach (var currentChild in children)
            {
                currentChild.Render(spriteBatch, newTransform);
            }
            base.Render(spriteBatch, transform);
        }

        public override Vector4 GetBounds()
        {
            float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
            foreach(var currentChild in children)
            {
                if (currentChild == null)
                    continue;
                Vector4 childBounds = currentChild.GetBounds();
                minX = Math.Min(minX, childBounds.X);
                maxX = Math.Max(maxX, childBounds.Z);
                minY = Math.Min(minY, childBounds.Y);
                maxY = Math.Max(maxY, childBounds.W);
            }

            Matrix matrix = new Matrix(minX, minY, 0, 1, maxX, minY, 0, 1, minX, maxY, 0, 1, maxX, maxY, 0, 1);
            
            matrix *= transformMatrix;
            int i;
            for (i = 1, minX = matrix[0, 0], maxX = matrix[0, 0], minY = matrix[0, 1], maxY = matrix[0, 1]; i < 4; ++i)
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

