using UnityEngine;
using System.Collections;
using MagicForestClient;
using System.Collections.Generic;
/*
 * name：UIDataCache；
 * function: ui数据存储类 此类为单例；
 * author: 李齐；
 * */
public class UIDataCache {

  static private UIDataCache m_Instance = new UIDataCache();
  static public UIDataCache Instance
  {
    get
    {
      return m_Instance;
    }
  }
  public void Init(GameObject rootWindow)
  {
  }
 

  public bool IsNewbie()
  {
    return true;
  }
  //判断当前场景是否为多人PVE
  
  void ChangeSceneId(int sceneId)
  {
    lastSceneId = curSceneId;
    curSceneId = sceneId;
  }
  void HandlerHomeNotice(string str)
  {
    homeNotice = str ;
  }
  private float time;
 
  public int curUnlockSceneId = 1011; //当前解锁 的普通关卡id
  public int curMasterUnlockSceneId = 2011;//当前解锁 的精英关卡id
  public bool justLogin = true;//刚刚登陆
  public int curSceneId;  //当前进入地图的id
  public int lastSceneId; //切换之前的场景Id
  public int curRank = -1;//名人赛当前排名
  public float curPlayerFightingScore = float.MinValue;//玩家当前战力
  public Vector3 MainSceneCameraPos = new Vector3(48.6937f,155.9267f,2.950846f);//用于保存登录场景时场景相机的数据
  public Vector3 MainSceneCameralocalEulerAngles = new Vector3(359.1326f, 0.0f, 0.0f);//和登录场景相机数据保持一致
  public bool m_IsSceneCameraInit = false;
  public bool isLoadingEnd = false;//技能是否laoding结束
  public bool needShowMarsLoading = false;//是否需要播放pvp前置动画
  public bool needShowChaosLoading = false;
  public bool masterRecord = true;// 是否需要名人战历史记录查询
  public int missionconfigID = -1;// 任务id
  public int treasureMapMoney = -1; // 试炼金钱，用于任务
  public int treasureCurrentClickIndex = -1; // 试炼金钱，用于任务当前点击
  public int vigor = 0;//角色当前的体力
  public bool isJinbiMatch = false;//是否在匹配金币副本
  public bool isShiLianMatch = false;//是否在匹配试炼副本
  public float curJinbiTime = 0f; // 金币副本匹配时间
  public float curShiLianTime = 0f;// 试炼副本匹配时间
  public bool isCGMoviePlayed = false;//是否播放过CG动画
  public string preMainCityOpenedUiName = "";//离开主城时，打开着的UI的名字
  #region Const Data
  public const int c_NewbieSceneId = 1001;
  #endregion
  public bool isSettingFogOpen = false;//用于记录设置里的雾效开关状况
  public bool isSettingEffectOpen = false;//用于记录设置里的特效开关状况
  public bool openSelectHero = false;// 用于记录是否进入选人，否则进入登录界面
  public string homeNotice = "";//进入主城公告
  public bool hasReceiveHomeNotice = false;//已经收到主城公告
  public List<int> unlickSkillList = new List<int>();//新解锁技能
  public bool isEnterCity = false;
  public bool isFirstCloseNextDayAward = false;//是否有生以来第一次关闭次登奖励
  public int taskSceneId = -1;//通过任务进入副本选择界面时打开的场景id
  public bool canNicknameSetVisible = true;   //用于名字版是否可以设置为可见
  public bool isWipeOutLevelUp = false; //扫荡升级
  public bool showPartnerPvpResult = false; // 显示伙伴pvp排名变化
  public bool hasLoadSelectRoleScene = false; //是否加载选人场景
  public bool isPlayerPrefsHasSetting = true; //是否已经有设置存档
  public string m_channelid = "";//渠道号
  public int nextDayAwardLeftTime = 0;//次登奖励领取剩余时间
  public System.DateTime nextDayAwardInitTime;//次登奖励初始化的时间
  public int GetNewbieSceneId()
  {
    return c_NewbieSceneId;
  }
  public MarsResultInfo marsResultInfo = new MarsResultInfo(false);
  void PvpResult(int result, int enemyheroid, int oldpoint, int point, int enemyoldpoint, int enemypoint, int oldrankid, int rankid, int damage, int enemydamage, int maxhitcount, int enemyhitcount, string enemynickname, bool isdare, bool isonlyone)
  {
    marsResultInfo.result = result;
    marsResultInfo.oldpoint = oldpoint;
    marsResultInfo.point = point;
    marsResultInfo.oldrankid = oldrankid;
    marsResultInfo.rankid = rankid;
    marsResultInfo.isShow = !isdare;
  }
  public int m_gowOtherInfoWinNums = 0;
  public int m_gowOtherInfoLoseNums = 0;
  void HandlerGowOtherInfo(int win, int lose)
  {
    m_gowOtherInfoWinNums = win;
    m_gowOtherInfoLoseNums = lose;
  }

  public bool IsLoadingResource = false; // 当前loading界面是否是刚进入游戏的资源加载界面
}
public struct MarsResultInfo
{
  public int result ;
  public int oldelo;
  public int elo;
  public int oldpoint;
  public int point;
  public int oldrankid;
  public int rankid;
  public bool isShow;
  public MarsResultInfo(bool show)
  {
    result = 0;
    oldelo = 0;
    elo = 0;
    oldpoint = 0;
    point = 0;
    oldrankid = 0;
    rankid = 0;
    isShow = show;
  }
}
