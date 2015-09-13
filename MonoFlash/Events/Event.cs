using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoFlash.Events
{
    public class Event
    {
        public static String ENTER_FRAME = "enterFrame";
        public static String ADDED_TO_STAGE = "addedToStage";
        public static String REMOVED_FROM_STAGE = "removedFromStage";

        public static String TOUCH_BEGIN = "touchBegin";
		public static String TOUCH_MOVE = "touchMove";
        public static String TOUCH_END = "touchEnd";

        public String type;

        public EventDispatcher currentTarget;
        public EventDispatcher target;

        public Vector2 position;

        public Event(String type)
        {
            this.type = type;
        }
    }
}
