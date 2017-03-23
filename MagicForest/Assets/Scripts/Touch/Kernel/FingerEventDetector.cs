using UnityEngine;
using System.Collections.Generic;
using MagicForestClient;

public class FingerEvent {
  FingerEventDetector detector;
  TouchManager.Finger finger;
  string name = string.Empty;
  bool startInUI = false;

  public string Name
  {
    get
    {
      return name;
    }
    set
    {
      name = value;
    }
  }

  public bool StartInUI
  {
    get
    {
      return startInUI;
    }
    set
    {
      startInUI = value;
    }
  }

  public FingerEventDetector Detector
  {
    get
    {
      return detector;
    }
    set
    {
      detector = value;
    }
  }

  public TouchManager.Finger Finger
  {
    get
    {
      return finger;
    }
    set
    {
      finger = value;
    }
  }

  public virtual GestureArgs ToGestureArgs()
  {
    GestureArgs e = new GestureArgs();
    e.startPositionX = this.Position.x;
    e.startPositionY = this.Position.y;
    e.deltaPositionX = 0;
    e.deltaPositionY = 0;
    e.positionX = this.Position.x;
    e.positionY = this.Position.y;

    e.startTime = Time.time;
    e.elapsedTime = 0;
    Vector3 pos = this.GetTouchToWorldPoint();
    e.gamePosX = pos.x;
    e.gamePosY = pos.y;
    e.gamePosZ = pos.z;
    e.name = this.Name;

    //GameObject go = DashFire.LogicForGfxThread.PlayerSelf;
    Vector3 srcPos = pos;
    Vector3 destPos = pos;
    //if (null != go)
    //{
    //  destPos = go.transform.position;
    //}
    e.towards = Geometry.GetYAngle(new ScriptRuntime.Vector2(destPos.x, destPos.z), new ScriptRuntime.Vector2(srcPos.x, srcPos.z));

    e.startInUI = this.StartInUI;

    e.moveType = TouchManager.curTouchState;

    return e;
  }

  public virtual Vector2 Position
  {
    get
    {
      return finger.Position;
    }
    set
    {
      throw new System.NotSupportedException("Setting position is not supported on " + this.GetType());
    }
  }

  public virtual Vector2 DeltaPosition
  {
    get
    {
      return finger.DeltaPosition;
    }
    set
    {
      throw new System.NotSupportedException("Setting DeltaPosition is not supported on " + this.GetType());
    }
  }

  public Vector3 GetTouchToWorldPoint()
  {
    if(null == Camera.main)
      return Vector3.zero;
    Vector3 cur_touch_worldpos = Vector3.zero;
    Vector3 cur_touch_pos = new Vector3(Position.x, Position.y, 0);
    Ray ray = Camera.main.ScreenPointToRay(cur_touch_pos);
    RaycastHit hitInfo;
    //int layermask = 1 << LayerMask.NameToLayer("AirWall");
    //layermask |= 1 << LayerMask.NameToLayer("SceneObjEffect");
    //layermask |= 1 << LayerMask.NameToLayer("SceneObj");
    //layermask = ~layermask;
    int layermask = (1 << LayerMask.NameToLayer("Terrains")) | (1 << LayerMask.NameToLayer("BuildingCollider"));
    if (Physics.Raycast(ray, out hitInfo, 200f, layermask)) {
      cur_touch_worldpos = hitInfo.point;
    }
    return cur_touch_worldpos;
  }

  public List<GameObject> GetRayObjectsByLayerName(string name)
  {
    if (null == Camera.main)
      return null;
    List<GameObject> go = new List<GameObject>(); 
    Vector3 cur_touch_pos = new Vector3(Position.x, Position.y, 0);
    Ray ray = Camera.main.ScreenPointToRay(cur_touch_pos);
    int layermask = 1 << LayerMask.NameToLayer(name);
    RaycastHit[] rch = Physics.RaycastAll(ray, 200f, layermask);
		for (int i = 0; i < rch.Length; ++i) {
			if (null != rch[i].collider.gameObject) {
				go.Add(rch[i].collider.gameObject);
			}
		}
		/*
    foreach (RaycastHit node in rch) {
      if (null != node.collider.gameObject) {
        go.Add(node.collider.gameObject);
      }
    }*/
    return go.Count > 0 ? go : null;
  }
}

public abstract class FingerEventDetector<T> : FingerEventDetector where T : FingerEvent, new() 
{
  List<T> fingerEventsList;
  public delegate void FingerEventHandler(T eventData);

  protected virtual T CreateFingerEvent()
  {
    return new T();
  }
  public override System.Type GetEventType()
  {
    return typeof(T);
  }
  protected override void Start()
  {
    try {
      base.Start();
      TouchManager.OnInputProviderChanged += TouchManager_OnInputProviderChanged;
      Init();
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }
  protected virtual void OnDestroy()
  {
    TouchManager.OnInputProviderChanged -= TouchManager_OnInputProviderChanged;
  }
  void TouchManager_OnInputProviderChanged()
  {
    Init();
  }
  protected virtual void Init()
  {
    Init(TouchManager.Instance.MaxFingers);
  }
  protected virtual void Init(int fingersCount)
  {
    fingerEventsList = new List<T>(fingersCount);
    for (int i = 0; i < fingersCount; ++i) {
      T e = CreateFingerEvent();
      e.Detector = this;
      e.Finger = TouchManager.GetFinger(i);
      fingerEventsList.Add(e);
    }
  }
  protected T GetEvent(TouchManager.Finger finger)
  {
    return GetEvent(finger.Index);
  }
  protected virtual T GetEvent(int fingerIndex)
  {
    return (null != fingerEventsList && fingerEventsList.Count > fingerIndex) ? fingerEventsList[fingerIndex] : null;
  }
}

public abstract class FingerEventDetector : MonoBehaviour 
{
  // -1 任何手指
  int FingerIndexFilter = -1;
  TouchManager.Finger activeFinger;

  protected abstract void ProcessFinger(TouchManager.Finger finger);
  public abstract System.Type GetEventType();

  protected virtual void Awake()
  {
  }

  protected virtual void Start()
  {
  }

  protected virtual void LateUpdate()
  {
    try {
      ProcessFingers();
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  protected virtual void ProcessFingers()
  {
    if (FingerIndexFilter >= 0 && FingerIndexFilter < TouchManager.Instance.MaxFingers) {
      if (!TouchManager.GetFinger(FingerIndexFilter).IsLocked())
      {
        ProcessFinger(TouchManager.GetFinger(FingerIndexFilter));
      }
    } else {
      for (int i = 0; i < TouchManager.Instance.MaxFingers; ++i) {
        if (!TouchManager.GetFinger(i).IsLocked())
        {
          ProcessFinger(TouchManager.GetFinger(i));
        }
      }
    }
  }

  private int GetCurTouchCount()
  {
    int nLockNum = 0;
    for (int i = 0; i < TouchManager.Instance.MaxFingers; i++)
    {
      if (TouchManager.GetFinger(i).IsLocked())
      {
        nLockNum++;
      }
    }

    return Input.touchCount - nLockNum;
  }

  private string CorrectionEventName(FingerEvent eventData)
  {
    ///
    if ("OnFingerUp" == eventData.Name) {
      TouchManager.Instance.GestureRecognizerSwitch(true);
    }
    ///
    bool canModify = false;
    if ("MouseInput" == TouchManager.Instance.InputProvider.name) {
      bool isRightButtonPress = Input.GetMouseButton(1);
      bool isRightButtonDown = Input.GetMouseButtonDown(1);
      bool isRightButtonUp = Input.GetMouseButtonUp(1);
      if (isRightButtonPress || isRightButtonDown || isRightButtonUp) {
        canModify = true;
      }
    } else {
      if (GetCurTouchCount() > 1) {
        canModify = true;
      }
    }
    if (canModify) {
      if ("OnFingerDown" == eventData.Name) {
        eventData.Name = "OnTwoFingerDown";
      } else if ("OnFingerUp" == eventData.Name) {
        eventData.Name = "OnTwoFingerUp";
      } else if ("OnFingerMove" == eventData.Name) {
        eventData.Name = "OnTwoFingerMove";
      }
    }
    return eventData.Name;
  }

  /// FireEvent
  protected void TrySendMessage(FingerEvent eventData)
  {
    eventData.Name = CorrectionEventName(eventData);
    TouchManager.FireEvent(eventData);
  }
}
