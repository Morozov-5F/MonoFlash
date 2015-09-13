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
        public bool visible;

        new public float ScaleX
        {
            get
            {
                return base.ScaleX;
            }
            set
            {
                base.ScaleX = value;
                // Negative scale
                for (int i = 0; i < children.Count; ++i)
                {
                    var currentChild = children[i]; 
                    currentChild.flippedX ^= flippedX;
                }
            }
        } 
        new public float ScaleY
        {
            get
            {
                return base.ScaleY;
            }
            set
            {
                base.ScaleY = value;
                // Negative scale
                for (int i = 0; i < children.Count; ++i)
                {
                    var currentChild = children[i]; 
                    currentChild.flippedY ^= flippedY;
                }
            }
        }

        public Sprite()
        {
            visible = true;

            children = new List<DisplayObject>();

            AddEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
            AddEventListener(Event.TOUCH_BEGIN, DispatchEventToHitChildren);
            AddEventListener(Event.TOUCH_MOVE, DispatchEventToHitChildren);
            AddEventListener(Event.TOUCH_END, DispatchEventToClickedChildren);
        }

        private void onAddedToStage(Event e)
        {
            foreach (var cC in children)
            {
                cC.stage = stage;
            }
        }

        private void DispatchEventToHitChildren(Event e)
        {
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                var touchEvent = new Event(e.type);
                touchEvent.position = child.GlobalToLocal(this.LocalToGlobal(e.position));
                if (child.HitTestPoint(touchEvent.position))
                {
                    child.isClicked = true;
                    child.DispatchEvent(touchEvent);
                }
            }
        }

        private void DispatchEventToClickedChildren(Event e)
        {
            for (int i = 0; i < children.Count; ++i)
            {
                var child = children[i];
                if (child == null)
                {
                    continue;
                }
                if (child.isClicked)
                {
                    var touchEvent = new Event(e.type);
                    touchEvent.position = child.GlobalToLocal(this.LocalToGlobal(e.position));     
                    child.isClicked = false;
                    child.DispatchEvent(touchEvent);                
                }
            }
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
            obj.parent = this;
            obj.DispatchEvent(new Event(Event.ADDED_TO_STAGE));
            obj.GlobalTransform = transformMatrix;
            return true;
        }

        /// <summary>
        /// Adds the child to sprite at given index
        /// </summary>
        /// <returns>true, if child can be attached, flase otherwise</returns>
        /// <param name="obj">DisplayObject to attach</param>
        /// <param name="index">Index to attach at</param>
        public bool AddChildAt(DisplayObject obj, int index)
        {
            if (obj.parent != null)
            {
                return false;
            }
            // Вставка перед объектом с индексом index
            children.Insert(index + 1, obj);
            obj.parent = this;
            obj.DispatchEvent(new Event(Event.ADDED_TO_STAGE));
            obj.GlobalTransform = transformMatrix;
            return true;
        }

        /// <summary>
        /// Gets the index of the child.
        /// </summary>
        /// <param name="obj">child to find</param>
        /// <returns>The child index at children</returns>
        public int GetChildIndex(DisplayObject obj)
        {
            return children.IndexOf(obj);
        }

        /// <summary>
        /// Gets the child at index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Child at given index</returns>
        public DisplayObject GetChildAt(int index)
        {
            // TODO: exceptions
            return children[index];
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
            obj.DispatchEvent(new Event(Event.REMOVED_FROM_STAGE));
            obj.parent = null;
            obj.stage = null;
            return true;
        }

        public override void Render(SpriteBatch spriteBatch, Matrix transform)
        {
            var newTransform = this.transformMatrix * transform;
            if (visible)
            {
                foreach (var currentChild in children)
                {
                    currentChild.Render(spriteBatch, newTransform);
                }
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
            //minX += pos.X; maxX += pos.X; minY += pos.Y; maxY += pos.Y;
            Matrix matrix = new Matrix(minX, minY, 0, 1, maxX, minY, 0, 1, minX, maxY, 0, 1, maxX, maxY, 0, 1);
            // TODO: multiplicate by this.transform
            matrix *= transformMatrix;
            //Matrix.CreateScale(new Vector3(ScaleX, ScaleY, 1)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) * Matrix.CreateTranslation(X, Y, 0) ;
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

