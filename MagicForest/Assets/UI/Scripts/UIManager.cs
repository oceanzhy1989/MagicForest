using System;
using UnityEngine;
using System.Collections.Generic;
using MagicForestClient;

public enum UILoadType : int
{
  DontLoad = -1,
  NoneActive = 0,
  Active = 1
}

public class UIManager
{
  public delegate void VoidDelegate();
  public delegate void StringDelegate(string name);
  public VoidDelegate OnAllUiLoadedDelegate;
  public VoidDelegate OnHideAllUIDelegate;
  public StringDelegate OnHideUiDelegate;
  public VoidDelegate OnTriggerNewbieGuideDelegate;

  private const int c_GameFrameWhenOpenFullUi = 10;    //当打开全屏UI时，降低游戏帧率
  private int m_GameFrameBackUp = 5;                //保存降低之前的帧率，用于恢复
  //public void Init()
  //{
  //  //数据的初始化需要在表格全部读取完毕开始
  //  uiConfigDataDic = UiConfigProvider.Instance.UiConfigMgr.GetData();
  //  UIBeginnerGuideManager.Instance.InitNewbieGuideData();
  //}
  //public void Init(GameObject rootWindow)
  //{
  //  m_RootWindow = rootWindow;
  //  DashFire.LogicForGfxThread.EventForGfx.Subscribe<string>("hide_ui", "ui", HideWindowByName);
  //  DashFire.LogicForGfxThread.EventForGfx.Subscribe("ge_toggle_all_ui", "ui", ToggleAllUI);
  //}

  //public GameObject rootWindow
  //{
  //  get { return m_RootWindow; }
  //}
  ///// <summary>
  ///// 给策划拍视频时临时增加的方法，不要调用
  ///// </summary>
  //private bool tempUiVisible = true;
  //private void ToggleAllUI()
  //{
  //  tempUiVisible = !tempUiVisible;
  //  SetAllUiVisible(tempUiVisible);
  //}
  //public void Clear()
  //{
  //  m_IsLoadedWindow.Clear();
  //  m_VisibleWindow.Clear();
  //  m_UnVisibleWindow.Clear();
  //  m_ExclusionWindow.Clear();
  //  m_ExclusionWindowStack.Clear();
  //  IsUIVisible = true;
  //  UIDataCache.Instance.canNicknameSetVisible = true;
  //}
  //获取已经加载的窗口GameObject
  public GameObject GetWindowGoByName(string windowName)
  {
    if (string.IsNullOrEmpty(windowName))
      return null;
    if (m_IsLoadedWindow.ContainsKey(windowName.Trim()))
      return m_IsLoadedWindow[windowName];
    else
    {
        GameObject obj = Resources.Load(windowName) as GameObject;
        if(null != obj)
        {
            m_IsLoadedWindow.Add(windowName, obj);
            return obj;
        }
    }
    return null;
  }
    //如果windowName还没加载，则主动加载一个并返回
    //public GameObject TryGetWindowGameObject(string windowName)
    //{
    //  if (string.IsNullOrEmpty(windowName))
    //    return null;
    //  if (m_IsLoadedWindow.ContainsKey(windowName.Trim()))
    //    return m_IsLoadedWindow[windowName];
    //  UiConfig uiCfg = GetUiConfigByName(windowName);
    //  if (null != uiCfg) {
    //    return LoadDeactiveWindow(uiCfg.m_WindowName, UICamera.mainCamera);
    //  }
    //  return null;
    //}
    ////获取UI的路径
    //public string GetPathByName(string windowName)
    //{
    //  UiConfig uiCfg = GetUiConfigByName(windowName);
    //  if (uiCfg != null) {
    //    return uiCfg.m_WindowPath;
    //  }
    //  return null;
    //}
    //public UiConfig GetUiConfigByName(string name)
    //{
    //  foreach (var pair in uiConfigDataDic) {
    //    UiConfig uiCfg = pair.Value as UiConfig;
    //    if (uiCfg != null && uiCfg.m_WindowName == name) {
    //      return uiCfg;
    //    }
    //  }
    //  return null;
    //}
    //public GameObject LoadWindowByName(string windowName, Camera cam)
    //{
    //  GameObject window = null;
    //  UiConfig uiCfg = GetUiConfigByName(windowName);
    //  if (null != uiCfg) {
    //    window = DashFire.ResourceSystem.GetSharedResource(uiCfg.m_WindowPath) as GameObject;
    //    if (null != window) {
    //      window = NGUITools.AddChild(m_RootWindow, window);
    //      Vector3 screenPos = CalculateUiPos(uiCfg.m_OffsetLeft, uiCfg.m_OffsetRight, uiCfg.m_OffsetTop, uiCfg.m_OffsetBottom);
    //      if (null != window && cam != null)
    //        window.transform.position = cam.ScreenToWorldPoint(screenPos);
    //      string name = uiCfg.m_WindowName;
    //      while (m_IsLoadedWindow.ContainsKey(name)) {
    //        name += "ex";
    //      }
    //      m_IsLoadedWindow.Add(name, window);
    //      if (!m_VisibleWindow.ContainsKey(name))
    //        m_VisibleWindow.Add(name, window);
    //      return window;
    //    } else {
    //      Debug.Log("!!!load " + uiCfg.m_WindowPath + " failed");
    //    }
    //  } else {
    //    Debug.Log("!!!load " + windowName + " failed");
    //  }
    //  return null;
    //}

    ////加载窗口，默认不显示()
    //private GameObject LoadDeactiveWindow(string windowName, Camera cam)
    //{
    //  GameObject window = null;
    //  UiConfig uiCfg = GetUiConfigByName(windowName);
    //  if (null != uiCfg) {
    //    window = DashFire.ResourceSystem.GetSharedResource(uiCfg.m_WindowPath) as GameObject;
    //    if (null != window) {
    //      //DashFire.ResourceSystem.GetSharedResource;
    //      window = NGUITools.AddChild(m_RootWindow, window);
    //      Vector3 screenPos = CalculateUiPos(uiCfg.m_OffsetLeft, uiCfg.m_OffsetRight, uiCfg.m_OffsetTop, uiCfg.m_OffsetBottom);
    //      if (null != window && cam != null)
    //        window.transform.position = cam.ScreenToWorldPoint(screenPos);
    //      string name = uiCfg.m_WindowName;
    //      while (m_IsLoadedWindow.ContainsKey(name)) {
    //        name += "ex";
    //      }
    //      NGUITools.SetActive(window, false);
    //      m_IsLoadedWindow.Add(name, window);
    //      m_UnVisibleWindow.Add(name, window);
    //      return window;
    //    } else {
    //      Debug.Log("!!!load " + uiCfg.m_WindowPath + " failed");
    //    }
    //  } else {
    //    Debug.Log("!!!load " + windowName + " failed");
    //  }
    //  return null;
    //}
    //public void LoadAllWindows(int sceneType, Camera cam)
    //{
    //  if (null == m_RootWindow)
    //    return;
    //  foreach (var pair in uiConfigDataDic) {
    //    UiConfig info = pair.Value as UiConfig;
    //    if (null != info && info.m_ShowType != (int)(UILoadType.DontLoad) && info.m_OwnToSceneList.Contains(sceneType)) {
    //      //Debug.Log(info.m_WindowName);
    //      GameObject go = DashFire.ResourceSystem.GetSharedResource(info.m_WindowPath) as GameObject;
    //      if (go == null) {
    //        Debug.Log("!!!Load ui " + info.m_WindowPath + " failed.");
    //        continue;
    //      }
    //      if (info.m_WindowName == "ChatFloatPanel") {
    //        Debug.Log("!!!Load ChatFloatPanel ");
    //      }
    //      GameObject child = NGUITools.AddChild(m_RootWindow, go);
    //      if (info.m_ShowType == (int)(UILoadType.Active)) {
    //        NGUITools.SetActive(child, true);
    //        if (!m_VisibleWindow.ContainsKey(info.m_WindowName)) {
    //          m_VisibleWindow.Add(info.m_WindowName, child);
    //        }
    //      } else {
    //        NGUITools.SetActive(child, false);
    //        if (!m_UnVisibleWindow.ContainsKey(info.m_WindowName)) {
    //          m_UnVisibleWindow.Add(info.m_WindowName, child);
    //        }
    //      }
    //      if (!m_IsLoadedWindow.ContainsKey(info.m_WindowName)) {
    //        m_IsLoadedWindow.Add(info.m_WindowName, child);
    //      }
    //      Vector3 screenPos = CalculateUiPos(info.m_OffsetLeft, info.m_OffsetRight, info.m_OffsetTop, info.m_OffsetBottom);
    //      if (null != child && cam != null)
    //        child.transform.position = cam.ScreenToWorldPoint(screenPos);
    //    }
    //  }
    //  IsUIVisible = true;
    //}
    //public void UnLoadAllWindow()
    //{
    //  if (UIDataCache.Instance.LastSceneIsCity() && !UIDataCache.Instance.IsCity() && !UIDataCache.Instance.IsServerSelect()) {
    //    //即将进入副本场景
    //    CacheMaincityOpenedUi();
    //  } else {
    //    //切换账号
    //    if (UIDataCache.Instance.IsServerSelect())
    //      UIDataCache.Instance.preMainCityOpenedUiName = "";
    //  }
    //  //每一个订阅事件的窗口UI都需要一个UnSubscribe函数用于消除事件
    //  LogicForGfxThread.EventForGfx.Publish("ge_ui_unsubscribe", "ui");
    //  foreach (var pair in m_IsLoadedWindow) {
    //    var window = pair.Value;
    //    if (null != window)
    //      //NGUIDebug.Log(window.name);
    //      NGUITools.DestroyImmediate(window);
    //  }
    //  Clear();
    //}
    ////卸载窗口
    //public void UnLoadWindowByName(string name)
    //{
    //  GameObject go = GetWindowGoByName(name);
    //  if (go != null) {
    //    NGUITools.Destroy(go);
    //    if (m_IsLoadedWindow.ContainsKey(name))
    //      m_IsLoadedWindow.Remove(name);
    //    if (m_VisibleWindow.ContainsKey(name))
    //      m_VisibleWindow.Remove(name);
    //    if (m_UnVisibleWindow.ContainsKey(name))
    //      m_UnVisibleWindow.Remove(name);
    //  }
    //}
    //public void ShowWindowByName(string windowName)
    //{
    //  try {
    //    if (windowName == null)
    //      return;
    //    if (windowName == "ChatFloatPanel") {
    //      Debug.Log("!!!Load ChatFloatPanel ");
    //    }
    //    if (m_VisibleWindow.ContainsKey(windowName))
    //      return;
    //    UiConfig uiCfg = GetUiConfigByName(windowName);
    //    if (m_UnVisibleWindow.ContainsKey(windowName)) {
    //      GameObject window = m_UnVisibleWindow[windowName];
    //      if (null != window) {
    //        NGUITools.SetActive(window, true);
    //        m_VisibleWindow.Add(windowName, window);
    //        m_UnVisibleWindow.Remove(windowName);
    //      }
    //    } else {
    //      //走到这里正常情况为Dynamic类型的UI
    //      if (uiCfg != null && uiCfg.m_IsDynamic) {
    //        LoadWindowByName(uiCfg.m_WindowName, UICamera.mainCamera);
    //      }
    //    }
    //    if (uiCfg != null && uiCfg.m_IsExclusion == true) {
    //      CloseExclusionWindow(windowName);
    //      //如果是全屏UI 游戏帧率降到5
    //      //ReduceGameFrame();
    //    }
    //    OpenBtnsPanel(true);
    //  } catch (Exception ex) {
    //    DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    //  }
    //}
    //void OpenBtnsPanel(bool open)
    //{
    //  GameObject btnsPanel = UIManager.Instance.GetWindowGoByName("MainCityBtnsPanel");
    //  if (btnsPanel == null)
    //    return;
    //  btnsPanel = btnsPanel.transform.Find("panel").gameObject;
    //  if (btnsPanel.GetComponent<MainCityBtnsPanel>().isOpen == !open) {
    //    return;
    //  }
    //  btnsPanel.GetComponent<MainCityBtnsPanel>().isOpen = open;
    //  btnsPanel.GetComponent<MainCityBtnsPanel>().openAndClose();
    //}
    //public void ShowForceGuide(string windowName)//系统引导专用
    //{
    //  try {
    //    if (windowName == null)
    //      return;
    //    if (m_VisibleWindow.ContainsKey(windowName))
    //      return;
    //    UiConfig uiCfg = GetUiConfigByName(windowName);
    //    if (m_UnVisibleWindow.ContainsKey(windowName)) {
    //      GameObject window = m_UnVisibleWindow[windowName];
    //      if (null != window) {
    //        NGUITools.SetActive(window, true);
    //        m_VisibleWindow.Add(windowName, window);
    //        m_UnVisibleWindow.Remove(windowName);
    //      }
    //    } else {
    //      //走到这里正常情况为Dynamic类型的UI
    //      if (uiCfg != null && uiCfg.m_IsDynamic) {
    //        LoadWindowByName(uiCfg.m_WindowName, UICamera.mainCamera);
    //      }
    //    }
    //    if (uiCfg != null && uiCfg.m_IsExclusion == true) {
    //      CloseExclusionWindow(windowName);
    //    }
    //  } catch (Exception ex) {
    //    DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    //  }
    //}
    //private void ShowWindow(string windowName)
    //{
    //  if (windowName == null)
    //    return;
    //  if (windowName == "ChatFloatPanel") {
    //    Debug.Log("!!!Load ChatFloatPanel ");
    //  }
    //  if (m_VisibleWindow.ContainsKey(windowName))
    //    return;
    //  if (m_UnVisibleWindow.ContainsKey(windowName)) {
    //    GameObject window = m_UnVisibleWindow[windowName];
    //    if (null != window) {
    //      NGUITools.SetActive(window, true);
    //      m_VisibleWindow.Add(windowName, window);
    //      m_UnVisibleWindow.Remove(windowName);
    //    }
    //  } else {
    //    //走到这里，正常情况下为Dynamic类型UI
    //    LoadWindowByName(windowName, UICamera.mainCamera);
    //  }
    //}
    //public void HideWindowByName(string windowName)
    //{
    //  try {
    //    if (windowName == null)
    //      return;
    //    if (m_UnVisibleWindow.ContainsKey(windowName))
    //      return;
    //    UiConfig uiCfg = GetUiConfigByName(windowName);
    //    if (null != uiCfg && uiCfg.m_IsDelayClose) {
    //      GameObject window = UIManager.Instance.GetWindowGoByName(windowName);
    //      if (window != null) {
    //        OpenAndCloseUi[] oacs = window.GetComponentsInChildren<OpenAndCloseUi>();
    //        if (oacs != null && oacs.Length > 0) {
    //          for (int i = 0; i < oacs.Length; i++) {
    //            if (i == 0)
    //              oacs[i].OnCloseUI(windowName);
    //            else
    //              oacs[i].PlayClose();
    //          }
    //        } else {
    //          HideWindowByNameDelay(windowName);
    //        }
    //      }
    //    } else {
    //      HideWindowByNameDelay(windowName);
    //    }
    //  } catch (Exception ex) {
    //    DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    //  }
    //}

    //public void HideWindowByNameDelay(string windowName)
    //{
    //  if (windowName == null)
    //    return;
    //  if (m_UnVisibleWindow.ContainsKey(windowName))
    //    return;
    //  UiConfig uiCfg = GetUiConfigByName(windowName);
    //  if (m_VisibleWindow.ContainsKey(windowName)) {
    //    if (uiCfg != null && uiCfg.m_IsDynamic) {
    //      //如果为动态类型则卸载
    //      UnLoadWindowByName(windowName);
    //    } else {
    //      GameObject window = m_VisibleWindow[windowName];
    //      if (null != window) {
    //        NGUITools.SetActive(window, false);
    //        if (OnHideUiDelegate != null) {
    //          OnHideUiDelegate(windowName);
    //        }
    //        m_UnVisibleWindow.Add(windowName, window);
    //        m_VisibleWindow.Remove(windowName);
    //      }
    //    }
    //  }
    //  if (uiCfg != null && uiCfg.m_IsExclusion == true) {
    //    OpenExclusionWindow(windowName);
    //  }
    //}
    ////调用该函数需要处理m_VisibleWindow 和m_UnVisibleWindow
    //private void HideWindow(string windowName)
    //{
    //  if (windowName == null)
    //    return;
    //  if (m_UnVisibleWindow.ContainsKey(windowName))
    //    return;
    //  if (m_VisibleWindow.ContainsKey(windowName)) {
    //    GameObject window = m_VisibleWindow[windowName];
    //    if (null != window) {
    //      NGUITools.SetActive(window, false);
    //    }
    //    m_UnVisibleWindow.Add(windowName, m_VisibleWindow[windowName]);
    //    m_VisibleWindow.Remove(windowName);
    //  }
    //}
    //public void ToggleWindowVisible(string windowName)
    //{
    //  if (IsWindowVisible(windowName)) {
    //    HideWindowByName(windowName);
    //  } else {
    //    ShowWindowByName(windowName);
    //  }
    //}
    //public bool IsWindowVisible(string windowName)
    //{
    //  if (m_VisibleWindow.ContainsKey(windowName))
    //    return true;
    //  else {
    //    return false;
    //  }
    //}
    ////关闭除windowName之外的所有窗口
    //public void CloseExclusionWindow(string windowName)
    //{
    //  UiConfig exclusionWndCfg = GetUiConfigByName(windowName);
    //  if (null == exclusionWndCfg) return;
    //  List<string> needPushInStackWindows = new List<string>();//需要进栈的窗口
    //  List<UiConfig> allHideWindows = new List<UiConfig>(); //关闭的窗口
    //  foreach (var pair in m_VisibleWindow) {
    //    var name = pair.Key;
    //    UiConfig cfg = GetUiConfigByName(name);
    //    if (name != windowName && cfg != null && cfg.m_Group == exclusionWndCfg.m_Group) {
    //      if (!m_DontPushIntoStack.Contains(name)) {
    //        needPushInStackWindows.Add(name);
    //      }
    //      allHideWindows.Add(cfg);
    //    }
    //  }
    //  if (needPushInStackWindows.Count > 0)
    //    m_ExclusionWindowStack.Push(needPushInStackWindows);
    //  for (int index = 0; index < allHideWindows.Count; ++index) {
    //    if (allHideWindows[index].m_IsDynamic && !m_DontUnloadWhileDeactiveByOtherWnds.Contains(allHideWindows[index].m_WindowName)) {
    //      UnLoadWindowByName(allHideWindows[index].m_WindowName);
    //    } else {
    //      HideWindow(allHideWindows[index].m_WindowName);
    //    }
    //  }
    //  allHideWindows.Clear();
    //}
    ////打开之前关闭的窗口
    //public void OpenExclusionWindow(string windowName)
    //{
    //  //
    //  while (m_ExclusionWindowStack.Count > 0) {
    //    m_ExclusionWindow = m_ExclusionWindowStack.Pop();
    //    if (m_ExclusionWindow == null)
    //      continue;
    //    for (int i = 0; i < m_ExclusionWindow.Count; i++) {
    //      if (!string.IsNullOrEmpty(m_ExclusionWindow[i])) {
    //        ShowWindow(m_ExclusionWindow[i]);
    //      }
    //    }
    //    m_ExclusionWindow.Clear();
    //    break;
    //  }
    //  //如果所有的全屏UI都被关闭了，恢复游戏帧率
    //  if (null != m_ExclusionWindowStack && m_ExclusionWindowStack.Count == 0) {
    //    //ResetGameFrame();
    //  }
    //}
    //public void SetAllUiVisible(bool isVisible)
    //{
    //  //需要把名字版也关掉
    //  LogicForGfxThread.EventForGfx.Publish("ge_setting_show_nick_name", "setting", isVisible);
    //  if (isVisible) {
    //    //ShowWindowByName("MainCityBtnsPanel");
    //    TouchManager.TouchEnable = true;
    //    OpenExclusionWindow("");
    //  } else {
    //    //HideWindowByName("MainCityBtnsPanel");
    //    TouchManager.TouchEnable = false;
    //    CloseExclusionWindow("SceneListenUI");
    //  }
    //  IsUIVisible = isVisible;
    //  if (IsUIVisible) {
    //    //播放剧情过程中，摇杆被强制隐藏（不允许其它操作打开摇杆，）剧情播放结束时,再打开摇杆
    //    ShowJoystick(true);
    //  }
    //  NicknameAndMoney(isVisible);
    //}
    //public void ShowJoystick(bool enable)
    //{
    //  //剧情播放过程中，摇杆被隐藏
    //  if (enable && IsUIVisible) {
    //    ShowWindowByName("JoyStickPanel");
    //  }
    //  if (!enable)
    //    HideWindowByName("JoyStickPanel");
    //}
    ///*判断是否有全屏UI显示*/
    //public bool IsAnyExclusionWindowVisble()
    //{
    //  foreach (var pair in m_VisibleWindow) {
    //    var name = pair.Key;
    //    UiConfig uiCfg = GetUiConfigByName(name);
    //    if (uiCfg != null && uiCfg.m_IsExclusion)
    //      return true;
    //  }
    //  return false;
    //}
    ////缓存主城最后显示的窗口
    //private void CacheMaincityOpenedUi()
    //{
    //  UIDataCache.Instance.preMainCityOpenedUiName = "";
    //  foreach (KeyValuePair<string, GameObject> pair in m_VisibleWindow) {
    //    string name = pair.Key;
    //    if (null != m_VisibleWindow[name] && !m_DontOpenWhileReturnMaincity.Contains(name)) {
    //      UiConfig uiCfg = GetUiConfigByName(name);
    //      //只保存全屏的，非全屏的不予保存
    //      if (null != uiCfg && uiCfg.m_IsExclusion) {
    //        UIDataCache.Instance.preMainCityOpenedUiName = name;
    //        break;
    //      }
    //    }
    //  }
    //}
    ///*再次回到主城时，打上上次打开的窗口*/
    //public void ShowPreMainCityOpenedUi()
    //{
    //  if (UIDataCache.Instance.LastSceneIsCity()) {
    //    UIDataCache.Instance.preMainCityOpenedUiName = "";
    //    return;
    //  }
    //  if (UIDataCache.Instance.preMainCityOpenedUiName.Contains("TrialIntro")) {
    //    UIDataCache.Instance.preMainCityOpenedUiName = "";
    //  }
    //  if (!string.IsNullOrEmpty(UIDataCache.Instance.preMainCityOpenedUiName)) {
    //    if (UIDataCache.Instance.preMainCityOpenedUiName.Contains("ChapterExDouble")) {
    //      ShowWindowByName(UIDataCache.Instance.preMainCityOpenedUiName);
    //    } else if (!UIDataCache.Instance.preMainCityOpenedUiName.Contains("ChapterEx")) {
    //      ShowWindowByName(UIDataCache.Instance.preMainCityOpenedUiName);
    //    }
    //  }
    //  UIDataCache.Instance.preMainCityOpenedUiName = "";
    //}
    ////返回当前打开着的全屏UI
    //internal GameObject GetOpeningExclusionWnd()
    //{
    //  foreach (KeyValuePair<string, GameObject> pair in m_VisibleWindow) {
    //    string name = pair.Key;
    //    UiConfig cfg = GetUiConfigByName(name);
    //    if (null != cfg && cfg.m_IsExclusion && null != m_VisibleWindow[name]) {
    //      return m_VisibleWindow[name];
    //    }
    //  }
    //  return null;
    //}
    //void NicknameAndMoney(bool vis)
    //{
    //  if (m_RootWindow != null) {
    //    Transform tf = m_RootWindow.transform.Find("DynamicWidget");
    //    if (tf != null) {
    //      NGUITools.SetActive(tf.gameObject, vis);
    //    }
    //    GameObject go = UIManager.Instance.GetWindowGoByName("FightUI");
    //    if (go != null) {
    //      PveFightInfo pfi = go.GetComponent<PveFightInfo>();
    //      if (pfi != null) {
    //        pfi.SetActive(vis);
    //      }
    //    }
    //    tf = m_RootWindow.transform.Find("ScreenScrollTip");
    //    if (tf != null) {
    //      NGUITools.SetActive(tf.gameObject, vis);
    //    }
    //  }
    //}
    //#region CalculateUiPos
    //public Vector3 CalculateUiPos(float offsetL, float offsetR, float offsetT, float offsetB)
    //{
    //  float screen_width = 0;
    //  float screen_height = 0;
    //  if (UICamera.mainCamera != null) {
    //    screen_width = UICamera.mainCamera.pixelRect.width;
    //    screen_height = UICamera.mainCamera.pixelRect.height;
    //  } else {
    //    screen_width = Screen.width;
    //    screen_height = Screen.height;
    //  }

    //  Vector3 screenPos = new Vector3();
    //  if (offsetL == -1 && offsetR == -1) {
    //    screenPos.x = screen_width / 2;
    //  } else {
    //    if (offsetL != -1)
    //      screenPos.x = offsetL;
    //    else {
    //      screenPos.x = screen_width - offsetR;
    //    }
    //  }
    //  if (offsetT == -1 && offsetB == -1) {
    //    screenPos.y = screen_height / 2;
    //  } else {
    //    if (offsetT != -1) {
    //      screenPos.y = screen_height - offsetT;
    //    } else {
    //      screenPos.y = offsetB;
    //    }
    //  }
    //  screenPos.z = 0;
    //  return screenPos;
    //}
    //#endregion
    static private UIManager m_Instance = new UIManager();
    static public UIManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
    //#region GetItemProtetyStr(float data, int type)
    //static public string GetItemProtetyStr(float data, int type)
    //{
    //  string str = "";
    //  switch (type) {
    //    case (int)DashFire.AttributeConfig.ValueType.AbsoluteValue:
    //      str = (data > 0.0f ? "+" : "") + (int)data;
    //      break;
    //    case (int)DashFire.AttributeConfig.ValueType.PercentValue:
    //      str = (data > 0.0f ? "+" : "") + (int)(data * 100) + "%";
    //      break;
    //    case (int)DashFire.AttributeConfig.ValueType.LevelRateValue:
    //      str = (data > 0.0f ? "+" : "") + (int)(data);
    //      break;
    //    default:
    //      str = "No This Item Type!";
    //      break;
    //  }
    //  return str;
    //}
    //static public float GetItemPropertyData(float data, int type)
    //{
    //  float dataf = data;
    //  switch (type) {
    //    case (int)DashFire.AttributeConfig.ValueType.AbsoluteValue:
    //      dataf = (float)(Mathf.FloorToInt(data));
    //      break;
    //    case (int)DashFire.AttributeConfig.ValueType.PercentValue:
    //      dataf = (float)(Mathf.FloorToInt(data * 100) / 100.0f);
    //      break;
    //    case (int)DashFire.AttributeConfig.ValueType.LevelRateValue:
    //      break;
    //    default:
    //      break;
    //  }
    //  return dataf;
    //}
    //#endregion
    ////加载名字版相机
    //public void LoadNickNameCamera()
    //{
    //  try {
    //    GameObject parent = null;
    //    if (Camera.main != null) {
    //      parent = Camera.main.gameObject;
    //    } else {
    //      parent = GameObject.Find("Main Camera");
    //    }
    //    if (null != parent) {
    //      GameObject go = ResourceSystem.GetSharedResource("UI/Common/NicknameCamera") as GameObject;
    //      go = GameObject.Instantiate(go) as GameObject;
    //      if (go != null && parent != null) {
    //        Transform t = go.transform;
    //        t.parent = parent.transform;
    //        t.localPosition = Vector3.zero;
    //        t.localRotation = Quaternion.identity;
    //        t.localScale = Vector3.one;
    //        Camera nCam = go.GetComponent<Camera>();
    //        Camera pCam = parent.GetComponent<Camera>();
    //        if (null != pCam && null != nCam) {
    //          nCam.fieldOfView = pCam.fieldOfView;
    //        }
    //      }
    //    }
    //  } catch (Exception ex) {
    //    DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    //  }
    //}

    //降低帧率
    private void ReduceGameFrame()
  {
    m_GameFrameBackUp = Application.targetFrameRate;
    Application.targetFrameRate = c_GameFrameWhenOpenFullUi;
  }
  //重置帧率
  private void ResetGameFrame()
  {
    if (Application.targetFrameRate == c_GameFrameWhenOpenFullUi)
      Application.targetFrameRate = m_GameFrameBackUp;
    m_GameFrameBackUp = -1;
  }
  static public int UIRootMinimumHeight = 640;
  static public int UIRootMaximunHeight = 768;
  static public UnityEngine.Color SkillDrectorColor = new UnityEngine.Color(255, 255, 255);
  static public List<GameObject> CheckItemForDelete = new List<GameObject>();
  static public float dragtime = 0.0f;
  public bool IsUIVisible = true;
  private GameObject m_RootWindow = null;
  private List<string> m_ExclusionWindow = new List<string>();
  private Dictionary<string, GameObject> m_IsLoadedWindow = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> m_VisibleWindow = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> m_UnVisibleWindow = new Dictionary<string, GameObject>();
  //public Dictionary<string, WindowInfo> m_WindowsInfoDic = new Dictionary<string, WindowInfo>();
  //MyDictionary<int, object> uiConfigDataDic = new MyDictionary<int, object>();
  private Stack<List<string>> m_ExclusionWindowStack = new Stack<List<string>>();
  private List<string> m_DontUnloadWhileDeactiveByOtherWnds = new List<string> {
    "Dialog",
    "Partner",
    "TaskAward",
    "SceneIntroduce",
    "DynamicFriend",
    "PlayerInfoView",
    "PPVPFighterIntro",
    "LotteryPreview",
  };
  private List<string> m_DontOpenWhileReturnMaincity = new List<string> {
  };
  private List<string> m_DontPushIntoStack = new List<string>{
    "CommVictoryScene",
  };
}