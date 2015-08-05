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
            return true;
        }
    }
}
