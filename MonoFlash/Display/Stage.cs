using System;
using System.Collections.Generic;
using System.Text;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    public class Stage : Sprite
    {
		public float MouseX, MouseY;
        public float StageWidth, StageHeight;

        public Stage()
        {
            parent = this;

			// MouseX, MouseY
			AddEventListener(Event.TOUCH_BEGIN, UpdateMousePosition);
			AddEventListener(Event.TOUCH_MOVE, UpdateMousePosition);
			AddEventListener(Event.TOUCH_END, UpdateMousePosition);
        }

		void UpdateMousePosition(Event e)
		{
			MouseX = e.position.X;
			MouseY = e.position.Y;
		}
    }
}
