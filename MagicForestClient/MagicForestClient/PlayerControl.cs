using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicForestClient
{
    public class PlayerControl
    {
        private static PlayerControl inst_ = new PlayerControl();
        public static PlayerControl Instance { get { return inst_; } }

        internal void Init()
        {
            EventDispatcher.Instance.ListenTouchEventImpl(TouchEvent.Cesture, this.TouchHandle);
        }

        private void TouchHandle(int what, GestureArgs e)
        {

        }
    }
}
