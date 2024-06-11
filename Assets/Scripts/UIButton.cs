using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class UIButtonAction
    {
        public Action Clicked = null;
        public Action Held = null;

        public UIButtonAction(Action clicked = null, Action held = null)
        {
            this.Clicked = clicked;
            this.Held = held;
        }
    }


}