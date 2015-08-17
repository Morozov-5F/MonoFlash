using System;
using System.Collections.Generic;
using System.Text;

namespace MonoFlash.Display
{
    public class Stage : Sprite
    {
        public float StageWidth
        {
            get
            {
                if (GameMain.screenManager != null)
                {
                    return width / GameMain.screenManager.ScaleX;
                }
                return width;
            }
            set
            {
                width = value;
            }
        }
        public float StageHeight
        {
            get
            {
                if (GameMain.screenManager != null)
                {
                    return height / GameMain.screenManager.ScaleY;
                }
                return height;
            }
            set
            {
                height = value;
            }
        }

        public Stage()
        {
            parent = this;
        }
    }
}
