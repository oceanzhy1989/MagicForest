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

        private GameObject m_PlayerSelf = null;
    }
}
