using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MagicForestClient
{
    public class LogicInterface
    {
        private static LogicInterface inst_ = new LogicInterface();
        public static LogicInterface Instance { get { return inst_; } }
        public GameObject PlayerSelf
        {
            get
            {
                if(m_PlayerSelf == null)
                {
                    UnityEngine.Object obj = Resources.Load("Characters/MainChar");
                    m_PlayerSelf = obj ? GameObject.Instantiate(obj) as GameObject : null;
                }
                return m_PlayerSelf;
            }
        }

        public class LogicParameters
        {
            public float fJoystickShowAreaRadius;
            public float fMaxAcceleration;
            public float fMaxSpeed;
            public float fResistance;
        }

        public static LogicParameters s_LogicParameters = new LogicParameters();
        private GameObject m_PlayerSelf = null;
    }
}
