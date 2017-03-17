using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicForestClient
{
    public enum TouchType : int
    {
        Move = 0,
        Regognizer,
        Attack,
    }

    public enum TouchEvent : int
    {
        Cesture = 0,
    }

    public enum InputType : int
    {
        Touch = 0,
        Joystick = 1,
    }

    public class GestureArgs
    {
        public GestureArgs()
        {
        }
        /// 开始位置
        public float startPositionX;
        public float startPositionY;
        // 位置改变量
        public float deltaPositionX;
        public float deltaPositionY;
        /// 当前位置
        public float positionX;
        public float positionY;
        /// 开始时间
        public float startTime;
        /// 花费时间
        public float elapsedTime;
        /// 开始游戏坐标
        public float startGamePosX;
        public float startGamePosY;
        public float startGamePosZ;
        /// 游戏中坐标
        public float gamePosX;
        public float gamePosY;
        public float gamePosZ;
        /// 游戏中寻路使用的坐标
        public float airWelGamePosX;
        public float airWelGamePosY;
        public float airWelGamePosZ;
        /// 手势名称
        public string name;
        /// 手势方向
        public float towards;
        /// 移动类型
        public TouchType moveType;
        /// 是否选中目标
        public int selectedObjID;
        /// 段数
        public int sectionNum;
        /// 输入类型
        public InputType inputType;
        // 
        public bool startInUI;
    }
}
