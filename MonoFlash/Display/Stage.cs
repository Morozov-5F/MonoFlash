using System;
using System.Collections.Generic;
using System.Text;
using MonoFlash.Events;

namespace MonoFlash.Display
{
    /// <summary>
    /// A base container for all sprites in applications.
    /// Have DisplayObject.TRANSFORM_ABSOLUTE transformMatrix 
    /// </summary>
    public class Stage : Sprite
    {
        /// <summary>
        /// The mouse x position at Stage.
        /// </summary>
        public float MouseX;
        /// <summary>
        /// The mouse y position at Stage
        /// </summary>
        public float MouseY;

        /// <summary>
        /// Device width and height
        /// </summary>
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
