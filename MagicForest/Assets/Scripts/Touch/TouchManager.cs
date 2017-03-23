using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicForestClient;

/// Touch Manager
public class TouchManager : MonoBehaviour 
{
  public static readonly RuntimePlatform[] TouchScreenPlatforms = { 
    RuntimePlatform.IPhonePlayer,
    RuntimePlatform.Android,
    RuntimePlatform.BlackBerryPlayer,
    RuntimePlatform.WP8Player,
  };

  public enum FingerPhase {
    None = 0,
    Begin,
    Moving,
    Stationary,
  }


  public static Gesture.EventHandler OnGestureHintEvent;
  public static Gesture.EventHandler OnGestureEvent;
  public static FingerEventDetector<FingerEvent>.FingerEventHandler OnFingerEvent;
  public delegate void EventHandler();
  public static EventHandler OnInputProviderChanged;

  public static void FireHintEvent(Gesture gesture)
  {
    if (!TouchEnable) return;
    if (!GestureEnable) return;

    if (OnGestureHintEvent != null)
      OnGestureHintEvent(gesture);
  }

  public static void FireEvent(Gesture gesture)
  {
    if (!TouchEnable) return;
    if (!GestureEnable) return;

    if (null != gesture.Recognizer && "OnEasyGesture" != gesture.Recognizer.EventMessageName) {
      //if ("OnSingleTap" == gesture.Recognizer.EventMessageName
      //  && (gesture.Position.y < Screen.height / 3 && gesture.Position.x < Screen.width / 4)) {
      //} else {
        GestureArgs e = gesture.ToGestureArgs();
        EventDispatcher.FireGestureEvent(e);
      //}
    }

    //if (PlayerControl.Instance.EnableSkillInput) {
    //  if (OnGestureEvent != null)
    //    OnGestureEvent(gesture);
    //}

    //Debug.Log("...input SkillTags : " + gesture.SkillTags + ", event : " + gesture.Recognizer.EventMessageName + ", state : " + curTouchState.ToString());
  }

  public static void FireEvent(FingerEvent eventData)
  {
    if (!TouchEnable) return;

    int scene_id = 1;
    if (scene_id > 0) {
      GestureArgs e = eventData.ToGestureArgs();
      EventDispatcher.FireGestureEvent(e);
    }

    if (OnFingerEvent != null)
      OnFingerEvent(eventData);

    //Debug.LogWarning("...input event : " + eventData.Name + ", State : " + curTouchState.ToString());
  }

  public static bool AllFingersMoving(params Finger[] fingers)
  {
    if (fingers.Length == 0)
      return false;

    foreach (Finger finger in fingers)
    {
      if (finger.Phase != FingerPhase.Moving)
        return false;
    }

    return true;
  }

  public static bool FingersMovedInOppositeDirections(Finger finger0, Finger finger1, float minDOT)
  {
    float dot = Vector2.Dot(finger0.DeltaPosition.normalized, finger1.DeltaPosition.normalized);
    return dot < minDOT;
  }

  public void GestureRecognizerSwitch(bool isOpen)
  {
    EasyRegognizer er = gameObject.GetComponent<EasyRegognizer>();
    if (isOpen) {
      if (null != er && false == er.enabled) {
        er.enabled = true;
      }
    } else {
      if (null != er && true == er.enabled) {
        er.enabled = false;
      }
    }
  }


  public bool aross = false;

  public bool unityremote = true;

  public InputProvider mouseInputPrefab;
  public InputProvider touchInputPrefab;

  public NGUIJoyStick JoyStickUI = null;

  public static MagicForestClient.TouchType curTouchState = MagicForestClient.TouchType.Regognizer;
  private static bool gestureEnable = true;
  public static bool GestureEnable
  {
    get
    {
      return gestureEnable;
    }
    set
    {
      gestureEnable = value;
    }
  }
  public static bool TouchEnable {get;set;}
  /// instance
  public static TouchManager Instance
  {
    get
    {
      return TouchManager.instance;
    }
  }

  void Init()
  {
    InitInputProvider();
  }


  InputProvider inputProvider;
  public InputProvider InputProvider
  {
    get
    {
      return inputProvider;
    }
  }

  public class InputProviderEvent 
  {
    public InputProvider inputPrefab;
  }

  public static bool IsTouchScreenPlatform(RuntimePlatform platform)
  {
    for (int i = 0; i < TouchScreenPlatforms.Length; ++i) {
      if (platform == TouchScreenPlatforms[i])
        return true;
    }
    return false;
  }

  void InitInputProvider()
  {
    InputProviderEvent e = new InputProviderEvent();
    if (IsTouchScreenPlatform(Application.platform)) {
      e.inputPrefab = touchInputPrefab;
    } else {
      e.inputPrefab = mouseInputPrefab;
    }
    InstallInputProvider(e.inputPrefab);
  }

  public void InstallInputProvider(InputProvider inputPrefab)
  {
    if (!inputPrefab) {
      //Debug.LogError("Invalid InputProvider (null)");
      return;
    }

    //Debug.Log("TouchManager: using " + inputPrefab.name);

    if (inputProvider) {
      Destroy(inputProvider.gameObject);
    }

    //inputProvider = ResourceSystem.NewObject(inputPrefab) as InputProvider;
    inputProvider = Instantiate(inputPrefab) as InputProvider;
    inputProvider.name = inputPrefab.name;
    inputProvider.transform.parent = this.transform;

    InitFingers(MaxFingers);

    if (OnInputProviderChanged != null) {
      OnInputProviderChanged();
    }
  }

  /// Finger
  public class Finger 
  {
    public int Index
    {
      get
      {
        return index;
      }
    }
    public bool IsDown
    {
      get
      {
        return phase != FingerPhase.None;
      }
    }
    public FingerPhase Phase
    {
      get
      {
        return phase;
      }
    }

    public FingerPhase PreviousPhase
    {
      get
      {
        return prevPhase;
      }
    }

    public bool WasDown
    {
      get
      {
        return prevPhase != FingerPhase.None;
      }
    }

    public bool IsMoving
    {
      get
      {
        return phase == FingerPhase.Moving;
      }
    }

    public bool WasMoving
    {
      get
      {
        return prevPhase == FingerPhase.Moving;
      }
    }

    public bool IsStationary
    {
      get
      {
        return phase == FingerPhase.Stationary;
      }
    }

    public bool WasStationary
    {
      get
      {
        return prevPhase == FingerPhase.Stationary;
      }
    }

    public bool Moved
    {
      get
      {
        return moved;
      }
    }

    public float StarTime
    {
      get
      {
        return startTime;
      }
    }

    public Vector2 StartPosition
    {
      get
      {
        return startPos;
      }
    }

    public Vector2 Position
    {
      get
      {
        return pos;
      }
    }

    public Vector2 PreviousPosition
    {
      get
      {
        return prevPos;
      }
    }

    public Vector2 DeltaPosition
    {
      get
      {
        return deltaPos;
      }
    }

    public float DistanceFromStart
    {
      get
      {
        return distFromStart;
      }
    }

    public bool IsFiltered
    {
      get
      {
        return filteredOut;
      }
    }

    public float TimeStationary
    {
      get
      {
        return elapsedTimeStationary;
      }
    }

    public List<GestureRecognizer> GestureRecognizers
    {
      get
      {
        return gestureRecognizers;
      }
    }

    public bool TryLockFinger(GestureRecognizer gr)
    {
      if (null != LockInGestureRecognizer)
      {
        return false;
      }
      LockInGestureRecognizer = gr;
      return true;
    }

    public bool TryUnLockFinger(GestureRecognizer gr)
    {
      if (gr != LockInGestureRecognizer)
      {
        return false;
      }
      LockInGestureRecognizer = null;
      return true;
    }

    public bool IsLocked()
    {
      return LockInGestureRecognizer != null;
    }

    public bool IsLockedBy(GestureRecognizer gr)
    {
      return LockInGestureRecognizer != null && LockInGestureRecognizer == gr;
    }

    /// privete
    int index = 0;
    FingerPhase phase = FingerPhase.None;
    FingerPhase prevPhase = FingerPhase.None;
    Vector2 pos = Vector2.zero;
    Vector2 startPos = Vector2.zero;
    Vector2 prevPos = Vector2.zero;
    Vector2 deltaPos = Vector2.zero;
    float startTime = 0;
    float lastMoveTime = 0;
    float distFromStart = 0;
    bool moved = false;
    bool filteredOut = true;
    Collider collider;
    Collider prevCollider;
    float elapsedTimeStationary = 0;

    GestureRecognizer LockInGestureRecognizer = null;

    List<GestureRecognizer> gestureRecognizers = new List<GestureRecognizer>();

    public Finger(int index)
    {
      this.index = index;
    }

    public override string ToString()
    {
      return "Finger" + index;
    }

    public static implicit operator bool(Finger finger)
    {
      return finger != null;
    }

    internal void Update(bool newDownState, Vector2 newPos)
    {
      if (filteredOut && !newDownState)
        filteredOut = false;

      if (!IsDown && newDownState && !TouchManager.instance.ShouldProcessTouch(index, newPos)) {
        filteredOut = true;
        newDownState = false;
      }

      prevPhase = phase;

      if (newDownState) {
        if (!WasDown) {
          phase = FingerPhase.Begin;

          pos = newPos;
          startPos = pos;
          prevPos = pos;
          deltaPos = Vector2.zero;
          moved = false;
          lastMoveTime = 0;

          startTime = Time.time;
          elapsedTimeStationary = 0;
          distFromStart = 0;
        } else {
          prevPos = pos;
          pos = newPos;
          distFromStart = Vector3.Distance(startPos, pos);
          deltaPos = pos - prevPos;

          if (deltaPos.sqrMagnitude > 0) {
            lastMoveTime = Time.time;
            phase = FingerPhase.Moving;
          } else if (!IsMoving || ((Time.time - lastMoveTime) > 0.05f)) {
            phase = FingerPhase.Stationary;
          }

          if (IsMoving) {
            moved = true;
          } else {
            if (!WasStationary) {
              elapsedTimeStationary = 0;
            } else {
              elapsedTimeStationary += Time.deltaTime;
            }
          }
        }
      } else {
        phase = FingerPhase.None;
      }
    }
  }

  public int MaxFingers
  {
    get
    {
      return inputProvider.MaxSimultaneousFingers;
    }
  }

  public static Finger GetFinger(int index)
  {
    return instance.fingers[index];
  }

  public static IFingerList Touches
  {
    get
    {
      return instance.touches;
    }
  }

  static List<GestureRecognizer> gestureRecognizers = new List<GestureRecognizer>();

  public static List<GestureRecognizer> RegisteredGestureRecognizers
  {
    get
    {
      return gestureRecognizers;
    }
  }

  public static void Register(GestureRecognizer recognizer)
  {
    if (gestureRecognizers.Contains(recognizer)) {
      return;
    }
    gestureRecognizers.Add(recognizer);
  }

  public static void Unregister(GestureRecognizer recognizer)
  {
    gestureRecognizers.Remove(recognizer);
  }


  void Awake()
  {
    try {
      CheckInit();
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  void Start()
  {
    try {
      if (aross) {
        DontDestroyOnLoad(this.gameObject);
      }
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }
  void OnDestroy()
  {
    instance = null;
  }

  void OnEnable()
  {
    try {
      CheckInit();
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  void CheckInit()
  {
    if (instance == null) {
      instance = this;
      Init();
      TouchEnable = true;
    } else if (instance != this) {
      Destroy(this.gameObject);
      TouchEnable = false;
      return;
    }
  }

  void Update()
  {
    try {
#if UNITY_EDITOR
      if (unityremote && Input.touchCount > 0 && inputProvider.GetType() != touchInputPrefab.GetType()) {
        InstallInputProvider(touchInputPrefab);
        unityremote = false;
        return;
      }
#endif
      if (inputProvider) {      
        UpdateFingers();        
      }
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  // singleton
  static TouchManager instance;

  Finger[] fingers;
  FingerList touches;

  void InitFingers(int count)
  {
    fingers = new Finger[count];
    for (int i = 0; i < count; ++i) {
      fingers[i] = new Finger(i);
    }
    touches = new FingerList();
  }

  void UpdateFingers()
  {
    touches.Clear();
    for (int i = 0; i < fingers.Length; ++i) {
      Finger finger = fingers[i];
      Vector2 pos = Vector2.zero;
      bool down = false;
      inputProvider.GetInputState(finger.Index, out down, out pos);
      finger.Update(down, pos);
      if (finger.IsDown) {       
          touches.Add(finger);
      }
    }
  }

  /// Global Input Filter
  /// <returns>true </returns>
  public delegate bool GlobalTouchFilterDelegate(int fingerIndex, Vector2 position);
  GlobalTouchFilterDelegate globalTouchFilterFunc;

  public static GlobalTouchFilterDelegate GlobalTouchFilter
  {
    get
    {
      return instance.globalTouchFilterFunc;
    }
    set
    {
      instance.globalTouchFilterFunc = value;
    }
  }

  protected bool ShouldProcessTouch(int fingerIndex, Vector2 position)
  {
    if (globalTouchFilterFunc != null) {
      return globalTouchFilterFunc(fingerIndex, position);
    }
    return true;
  }


  Transform[] fingerNodes;
  Transform CreateNode(string name, Transform parent)
  {
    GameObject go = new GameObject(name);
    go.transform.parent = parent;
    return go.transform;
  }
  void InitNodes()
  {
    int fingerCount = fingers.Length;

    if (fingerNodes != null) {
			for (int i = 0; i < fingerNodes.Length; i++) 
			{
				Destroy(fingerNodes[i].gameObject);
			}
			/*
      foreach (Transform fingerCompNode in fingerNodes) {
        Destroy(fingerCompNode.gameObject);
      }*/
    }
    fingerNodes = new Transform[fingerCount];
    for (int i = 0; i < fingerNodes.Length; ++i) {
      fingerNodes[i] = CreateNode("Finger" + i, this.transform);
    }
  }

  public interface IFingerList : IEnumerable<Finger> 
  {
    Finger this[int index]
    {
      get;
    }
    int Count
    {
      get;
    }

    bool RemoveAt(int index);

    bool Remove(Finger touch);

    Vector2 GetAverageStartPosition();

    Vector2 GetAveragePosition();

    Vector2 GetAveragePreviousPosition();

    float GetAverageDistanceFromStart();

    Finger GetOldest();

    bool AllMoving();

    bool MovingInSameDirection(float tolerance);
  }

  /// æ‰‹æŒ‡åˆ—è¡¨
  public class FingerList : IFingerList 
  {
    List<Finger> list;

    public FingerList()
    {
      list = new List<Finger>();
    }

    public FingerList(List<Finger> list)
    {
      this.list = list;
    }

    public Finger this[int index]
    {
      get
      {
        return list[index];
      }
    }

    public int Count
    {
      get
      {
        return list.Count;
      }
    }

    public IEnumerator<Finger> GetEnumerator()
    {
      return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Finger touch)
    {
      list.Add(touch);
    }

    public bool RemoveAt(int index)
    {
      if (index >= 0 && index < list.Count)
      {
        list.RemoveAt(index);
        return true;
      }
      return false;
    }

    public bool Remove(Finger touch)
    {
      return list.Remove(touch);
    }

    public bool Contains(Finger touch)
    {
      return list.Contains(touch);
    }

    public void AddRange(IEnumerable<Finger> touches)
    {
      list.AddRange(touches);
    }

    public void Clear()
    {
      list.Clear();
    }

    public delegate T FingerPropertyGetterDelegate<T>(Finger finger);

    public Vector2 AverageVector(FingerPropertyGetterDelegate<Vector2> getProperty)
    {
      Vector2 avg = Vector2.zero;
      if (Count > 0) {
        for (int i = 0; i < list.Count; ++i) {
          avg += getProperty(list[i]);
        }
        avg /= Count;
      }
      return avg;
    }

    public float AverageFloat(FingerPropertyGetterDelegate<float> getProperty)
    {
      float avg = 0;
      if (Count > 0) {
        for (int i = 0; i < list.Count; ++i) {
          avg += getProperty(list[i]);
        }
        avg /= Count;
      }
      return avg;
    }

    static FingerPropertyGetterDelegate<Vector2> delGetFingerStartPosition = GetFingerStartPosition;
    static FingerPropertyGetterDelegate<Vector2> delGetFingerPosition = GetFingerPosition;
    static FingerPropertyGetterDelegate<Vector2> delGetFingerPreviousPosition = GetFingerPreviousPosition;
    static FingerPropertyGetterDelegate<float> delGetFingerDistanceFromStart = GetFingerDistanceFromStart;

    static Vector2 GetFingerStartPosition(Finger finger)
    {
      return finger.StartPosition;
    }
    static Vector2 GetFingerPosition(Finger finger)
    {
      return finger.Position;
    }
    static Vector2 GetFingerPreviousPosition(Finger finger)
    {
      return finger.PreviousPosition;
    }
    static float GetFingerDistanceFromStart(Finger finger)
    {
      return finger.DistanceFromStart;
    }

    public Vector2 GetAverageStartPosition()
    {
      return AverageVector(delGetFingerStartPosition);
    }

    public Vector2 GetAveragePosition()
    {
      return AverageVector(delGetFingerPosition);
    }

    public Vector2 GetAveragePreviousPosition()
    {
      return AverageVector(delGetFingerPreviousPosition);
    }

    public float GetAverageDistanceFromStart()
    {
      return AverageFloat(delGetFingerDistanceFromStart);
    }

    public Finger GetOldest()
    {
      Finger oldest = null;
			for (int i = 0; i < list.Count; i++) 
			{
				if (oldest == null || (list[i].StarTime < oldest.StarTime)) {
					oldest = list[i];
				}
			}
			/*
      foreach (Finger finger in list) {
        if (oldest == null || (finger.StarTime < oldest.StarTime)) {
          oldest = finger;
        }
      }*/

      return oldest;
    }

    public bool MovingInSameDirection(float tolerance)
    {
      if (Count < 2) {
        return true;
      }
      float minDOT = Mathf.Max(0.1f, 1.0f - tolerance);
      Vector2 refDir = this[0].Position - this[0].StartPosition;
      refDir.Normalize();
      for (int i = 1; i < Count; ++i) {
        Vector2 dir = this[i].Position - this[i].StartPosition;
        dir.Normalize();
        if (Vector2.Dot(refDir, dir) < minDOT) {
          return false;
        }
      }
      return true;
    }

    public bool AllMoving()
    {
      if (Count == 0) {
        return false;
      }
      for (int i = 0; i < list.Count; ++i) {
        if (!list[i].IsMoving) {
          return false;
        }
      }
      return true;
    }
  }

  ///
  const float DESKTOP_SCREEN_STANDARD_DPI = 96; // ®¤ win7 dpi
  const float INCHES_TO_CENTIMETERS = 2.54f; //
  const float CENTIMETERS_TO_INCHES = 1.0f / INCHES_TO_CENTIMETERS; // 1 cm = 0.3937... ¯¸

  static float screenDPI = 0;

  public static float ScreenDPI
  {
    get
    {
      if (screenDPI <= 0) {
        screenDPI = Screen.dpi;
        if (screenDPI <= 0)
          screenDPI = DESKTOP_SCREEN_STANDARD_DPI;

#if UNITY_IPHONE

          if( UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.Unknown ||
              UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadUnknown ||
              UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneUnknown ) {
              // ipad mini 2
              if( Screen.width == 2048 && Screen.height == 1536 && screenDPI == 260 )
                  screenDPI = 326;
          }
#endif
      }
      return TouchManager.screenDPI;
    }
    set
    {
      TouchManager.screenDPI = value;
    }
  }

  public static float Convert(float distance, DistanceUnit fromUnit, DistanceUnit toUnit)
  {
    float dpi = ScreenDPI;
    float pixelDistance;

    switch (fromUnit) {
      case DistanceUnit.Centimeters:
        pixelDistance = distance * CENTIMETERS_TO_INCHES * dpi; // cm -> in -> px
        break;
      case DistanceUnit.Inches:
        pixelDistance = distance * dpi; // in -> px
        break;
      case DistanceUnit.Pixels:
      default:
        pixelDistance = distance;
        break;
    }

    switch (toUnit) {
      case DistanceUnit.Inches:
        return pixelDistance / dpi; // px -> in
      case DistanceUnit.Centimeters:
        return (pixelDistance / dpi) * INCHES_TO_CENTIMETERS;  // px -> in -> cm
      case DistanceUnit.Pixels:
        return pixelDistance;
    }

    return pixelDistance;
  }


  public static Vector2 Convert(Vector2 v, DistanceUnit fromUnit, DistanceUnit toUnit)
  {
    return new Vector2(Convert(v.x, fromUnit, toUnit),
                        Convert(v.y, fromUnit, toUnit));
  }
}

public enum DistanceUnit 
{
  Pixels,
  Inches,
  Centimeters,
}


public abstract class InputProvider : MonoBehaviour 
{

  public abstract int MaxSimultaneousFingers
  {
    get;
  }

  public abstract void GetInputState(int fingerIndex, out bool down, out Vector2 position);
}


public static class TouchExtensions 
{

  public static string Abreviation(this DistanceUnit unit)
  {
    switch (unit) {
      case DistanceUnit.Centimeters:
        return "cm";
      case DistanceUnit.Inches:
        return "in";
      case DistanceUnit.Pixels:
        return "px";
    }
    return unit.ToString();
  }


  public static float Convert(this float value, DistanceUnit fromUnit, DistanceUnit toUnit)
  {
    return TouchManager.Convert(value, fromUnit, toUnit);
  }


  public static float In(this float valueInPixels, DistanceUnit toUnit)
  {
    return valueInPixels.Convert(DistanceUnit.Pixels, toUnit);
  }


  public static float Centimeters(this float valueInPixels)
  {
    return valueInPixels.In(DistanceUnit.Centimeters);
  }

  public static float Inches(this float valueInPixels)
  {
    return valueInPixels.In(DistanceUnit.Inches);
  }

  /// Vector2
  public static Vector2 Convert(this Vector2 v, DistanceUnit fromUnit, DistanceUnit toUnit)
  {
    return TouchManager.Convert(v, fromUnit, toUnit);
  }


  public static Vector2 In(this Vector2 vecInPixels, DistanceUnit toUnit)
  {
    return vecInPixels.Convert(DistanceUnit.Pixels, toUnit);
  }

  public static Vector2 Centimeters(this Vector2 vecInPixels)
  {
    return vecInPixels.In(DistanceUnit.Centimeters);
  }


  public static Vector2 Inches(this Vector2 vecInPixels)
  {
    return vecInPixels.In(DistanceUnit.Inches);
  }
}
