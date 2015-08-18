using System;

namespace MonoFlash.Display
{
    /// <summary>
    /// A base container for all sprites in applications.
    /// Have DisplayObject.TRANSFORM_ABSOLUTE transformMatrix 
    /// </summary>
    public class Stage : Sprite
    {
        /// <summary>
        /// Device width and height
        /// </summary>
        public float StageWidth, StageHeight;
        
        public Stage()
        {
            parent = this;
        }
    }
}
