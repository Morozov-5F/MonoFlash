using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFlash.Display
{
    class Sprite : DisplayObject
    {
        public HashSet<DisplayObject> children;

        public Sprite()
        {
            children = new HashSet<DisplayObject>();
            scale = Vector2.One;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach(var currentChild in children)
            {
                if (currentChild == null)
                    continue;

                currentChild.ApplyTransformMatrix(TransformMatrix);
                currentChild.Draw(spriteBatch);
            }
        }

        public DisplayObject AddChild(DisplayObject obj)
        {
            if (!children.Add(obj) || obj == this)
            {
                return null;
            }
            obj.parent = this;
            return obj;
        }

        public DisplayObject RemoveChild(DisplayObject obj)
        {
            if (!children.Contains(obj))
            {
                return null;
            }
            children.Remove(obj);
            obj.parent = null;
            return obj;
        }
    }
}
