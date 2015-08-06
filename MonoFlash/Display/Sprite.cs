using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    class Sprite : DisplayObject
    {
        public List<DisplayObject> children;

        public Sprite()
        {
            children = new List<DisplayObject>();
        }

        public override void render(SpriteBatch spriteBatch)
        {
            if (!isVisible)
            {
                return;
            }
            foreach(DisplayObject child in children)
            {
                child.applyTransformMatrix(TransformMatrix);
                child.render(spriteBatch);
            }
            base.render(spriteBatch);
        }

        public bool addChild(DisplayObject child)
        {
            if (child == null || child.parent != null)
            {
                return false;
            }
            children.Add(child);
            child.parent = this;
            child.stage = stage;
            child.dispatchEvent(new Event(Event.ADDED_TO_STAGE));
            GetBounds();
            return true;
        }

        public bool removeChild(Sprite child)
        {
            if (child.parent == null)
            {
                return false;
            }
            if (child.parent != this)
            {
                return false;
            }
            child.parent = null;
            child.stage = null;
            children.Remove(child);
            child.dispatchEvent(new Event(Event.REMOVED_FROM_STAGE));
            GetBounds();
            return true;
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
            minX += pos.X; maxX += pos.X; minY += pos.Y; maxY += pos.Y;
            Matrix matrix = new Matrix(new Vector4(minX, minY, 0, 0), new Vector4(maxX, minY, 0, 0),
                                       new Vector4(minX, maxY, 0, 0), new Vector4(maxX, maxY, 0, 0));
            matrix *=  Matrix.CreateScale(new Vector3(scale, 1)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) ;
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
            width = (int)Math.Round(maxX - minX, 0);
            height = (int)Math.Round(maxY - minY, 0);
            return new Vector4(minX, minY, maxX, maxY);
        }
    }
}
