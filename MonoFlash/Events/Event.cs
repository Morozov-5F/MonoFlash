using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoFlash.Events
{
    class Event
    {
        public static String ENTER_FRAME = "enterFrame";
        public static String ADDED_TO_STAGE = "addedToStage";
        public static String REMOVED_FROM_STAGE = "removedFromStage";

        public String type;

        public EventDispatcher currentTarget;
        public EventDispatcher target;

        public Event(String type)
        {
            this.type = type;
        }
    }
}
