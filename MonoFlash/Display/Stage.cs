using System;
using System.Collections.Generic;
using System.Text;

namespace MonoFlash.Display
{
    class Stage : Sprite
    {
        public float stageWidth;
        public float stageHeight;

        public Stage()
        {
            parent = this;
        }
    }
}
