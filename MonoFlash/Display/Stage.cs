using System;

namespace MonoFlash.Display
{
    public class Stage : Sprite
    {
        public float StageWidth, StageHeight;
        
        public Stage()
        {
            parent = this;
        }
    }
}
