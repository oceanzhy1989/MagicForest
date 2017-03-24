using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MagicForestClient
{
    public class EventDispatcher
    {
        private static EventDispatcher inst_ = new EventDispatcher();
        public static EventDispatcher Instance { get { return inst_; } }


        public delegate void FingerStatus(GestureArgs e);
        public static FingerStatus OnFingerDown;
        public static FingerStatus OnFingerUp;

        public static void FireGestureEvent(GestureArgs args)
        {
            EventDispatcher.Instance.OnGestureEvent(args);
        }

        public void GameLogicTick()
        {
            GameObject player = LogicInterface.Instance.PlayerSelf;
            if(player != null)
            {
                //float speed = 0.0001f * m_CurJoyTargetPos.magnitude;
                //float speed = m_CurJoyTargetPos.magnitude > 65.0f ? 0.01f : 0.0f;
                //player.transform.position += new Vector3(speed * Mathf.Sin(m_CurJoyDir), speed * Mathf.Cos(m_CurJoyDir), 0);
                float fMaxForce = LogicInterface.s_LogicParameters.fMaxAcceleration;
                float force = m_CurJoyTargetPos.magnitude > LogicInterface.s_LogicParameters.fJoystickShowAreaRadius ? 
                    1.0f : m_CurJoyTargetPos.magnitude / LogicInterface.s_LogicParameters.fJoystickShowAreaRadius;
                force *= fMaxForce;
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    Vector2 vFPropellent = new Vector2(force * Mathf.Sin(m_CurJoyDir), force * Mathf.Cos(m_CurJoyDir));
                    Vector2 vFResist = -rb.velocity * rb.velocity.magnitude * LogicInterface.s_LogicParameters.fResistance;
                    rb.AddForce(vFPropellent + vFResist);

                    if(rb.velocity.magnitude > LogicInterface.s_LogicParameters.fMaxSpeed)
                    {
                        rb.velocity = rb.velocity.normalized * LogicInterface.s_LogicParameters.fMaxSpeed;
                    }
                }
            }
        }
        internal bool IsTouchPosChangedImpl()
        {
            if (m_LastTouchPos.x == m_CurTouchPos.x && m_LastTouchPos.y == m_CurTouchPos.y)
            {
                return false;
            }
            return true;
        }
        internal float GetTouchXImpl()
        {
            return m_CurTouchPos.x;
        }
        internal float GetTouchYImpl()
        {
            return m_CurTouchPos.y;
        }
        internal float GetTouchZImpl()
        {
            return m_CurTouchPos.z;
        }
        internal float GetTouchRayPointXImpl()
        {
            return m_TouchRayPoint.x;
        }
        internal float GetTouchRayPointYImpl()
        {
            return m_TouchRayPoint.y;
        }
        internal float GetTouchRayPointZImpl()
        {
            return m_TouchRayPoint.z;
        }
        public void ListenTouchEventImpl(TouchEvent c, MyAction<int, GestureArgs> handler)
        {
            if (m_TouchHandlers.ContainsKey((int)c))
            {
                m_TouchHandlers[(int)c] = handler;
            }
            else
            {
                m_TouchHandlers.Add((int)c, handler);
            }
        }
        private void Fire(int c, GestureArgs e)
        {
            MyAction<int, GestureArgs> handler;
            if (m_TouchHandlers.TryGetValue(c, out handler))
            {
                handler(c, e);
            }
        }
        public void OnGestureEvent(GestureArgs e)
        {
            if (null != e)
            {
                m_LastTouchPos = m_CurTouchPos;
                m_CurTouchPos = new Vector3(e.positionX, e.positionY, 0);
                m_TouchRayPoint = new Vector3(e.gamePosX, e.gamePosY, e.gamePosZ);
            }
            Fire((int)TouchEvent.Cesture, e);
            ///
            string ename = e.name;
            if (GestureEvent.OnFingerDown.ToString() == ename)
            {
                if (null != OnFingerDown)
                {
                    OnFingerDown(e);
                }
            }
            else if (GestureEvent.OnFingerUp.ToString() == ename)
            {
                if (null != OnFingerUp)
                {
                    OnFingerUp(e);
                }
            }
        }
        /// Joystick
        public void SetJoystickInfo(GestureArgs e)
        {
            if (null != e)
            {
                m_CurJoyDir = e.towards;
                m_CurJoyTargetPos.x = e.airWelGamePosX;
                m_CurJoyTargetPos.y = e.airWelGamePosY;
                m_CurJoyTargetPos.z = e.airWelGamePosZ;
            }
        }
        internal float GetJoystickDirImpl()
        {
            return m_CurJoyDir;
        }
        internal float GetJoystickTargetPosXImpl()
        {
            return m_CurJoyTargetPos.x;
        }
        internal float GetJoystickTargetPosYImpl()
        {
            return m_CurJoyTargetPos.y;
        }
        internal float GetJoystickTargetPosZImpl()
        {
            return m_CurJoyTargetPos.z;
        }

        ///
        private Vector3 m_LastTouchPos;
        private Vector3 m_CurTouchPos;
        private Vector3 m_TouchRayPoint;
        /// Joystick
        private float m_CurJoyDir;
        private Vector3 m_CurJoyTargetPos;

        private Dictionary<int, MyAction<int, GestureArgs>> m_TouchHandlers = new Dictionary<int, MyAction<int, GestureArgs>>();
    }
}
