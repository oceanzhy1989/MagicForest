using UnityEngine;

/// <summary>
/// 摇杆类--by liencheng
/// </summary>
public enum NGUIJoyStickEvent
{
  OnNone,
  OnJoystickMove,
  OnJoystickMoveEnd,
}
[ExecuteInEditMode]
public class NGUIJoyStick : MonoBehaviour
{

  #region Event
  public delegate void JoystickStartHandler(Vector2 move);
  public delegate void JoystickMoveHandler(Vector2 move);
  public delegate void JoystickMoveEndHandler(Vector2 move);
  public static JoystickStartHandler OnJoystickStart;
  public static JoystickMoveHandler OnJoystickMove;
  public static JoystickMoveEndHandler OnJoystickMoveEnd;
  #endregion
  /// <summary>
  /// Whether a copy of the item will be dragged instead of the item itself.
  /// </summary>
  //

  public UIWidget BackGround;

  //public bool need
  public bool cloneOnDrag = false;
  //摇杆图标transform
  public Transform stickTrans;
  //摇杆背景transform
  public Transform stickBackTrans;
  //摇杆默认位置transform
  public Transform stickPosTrans;
  //摇杆是否有区域限制
  public bool isAreaRestriction = false;
  //如果有显示区域限制，需要设置区域半径
  public float showAreaRadius = 100;
  [Tooltip("当摇杆控制超过一定范围时，摇杆整体跟着移动，当小于0时不移动")]
  public float MoveAreaRadius = 400;

  [Tooltip("摇杆随手指移动的差值参数")]
  public float Smooth = 10;

  [Tooltip("摇杆小于多少范围时认为没有移动")]
  public float Sensitive = -1f;
  //初始点，也是中心点
  private Vector3 m_StickCenterPos;
  //摇杆相对中心点的位置（手指位置-中心区域）
  private Vector2 m_StickMoveDelta = new Vector2();
  //通过计算记录当前点击（触摸）位置
  private Vector3 m_TempTouchPos = new Vector3();
  #region Common functionality
  protected Transform mTrans;
  protected Transform mParent;
  protected Collider mCollider;
  protected UIRoot mRoot;
  //protected int mTouchID = int.MinValue;

  private Vector2 mCurTouchPos = Vector2.zero;
  private Vector2 mLastouchPos = Vector2.zero;
  private Vector3 mBackTarPos;
  private Vector3 mStickTarPos;
  /// <summary>
  /// Cache the transform.
  /// </summary>
  void Start()
  {
    mTrans = transform;
    mCollider = GetComponent<Collider>();

    mParent = mTrans.parent;
    mRoot = NGUITools.FindInParents<UIRoot>(mParent);

    ResetStickPos();
  }
  void Update()
  {
#if UNITY_EDITOR
    if (!Application.isPlaying) {
      BoxCollider collider = this.transform.GetComponent<BoxCollider>();
      //Vector3 localPos = this.transform.localPosition;
      //collider.size = new Vector3(collider.size.x + localPos.x * 2, collider.size.y + localPos.y * 2, 0);
    }
#endif
    if(mBackTarPos != stickBackTrans.localPosition)
    {
      stickBackTrans.localPosition = Vector3.Lerp(stickBackTrans.localPosition, mBackTarPos, Time.deltaTime * Smooth);
    }
    if (mStickTarPos != stickTrans.localPosition)
    {
      stickTrans.localPosition = Vector3.Lerp(stickTrans.localPosition, mStickTarPos, Time.deltaTime * Smooth);
    }
  }

  private Vector3 mUIPostion;
  private Vector2 mUISize;
  public bool IsPosInJoyStick(Vector2 pos)
  {
    if (!BackGround)
      return false;

    if (Time.deltaTime != 0)
    {
      mUIPostion = BackGround.worldCenter;
      mUIPostion = UICamera.currentCamera.WorldToScreenPoint(mUIPostion);

      float scale = 1.0f;
      if (mRoot != null)
      {
        scale = Screen.height / (float)mRoot.activeHeight;
      }

      mUISize.x = (float)BackGround.width * scale * 0.5f;
      mUISize.y = (float)BackGround.height * scale * 0.5f;
    }

    float xl = Mathf.Abs(pos.x - mUIPostion.x);
    float yl = Mathf.Abs(pos.y - mUIPostion.y);
    if (xl <= mUISize.x && yl <= mUISize.y)
    {
      return true;
    }

    return false;
  }

  public void SetTouchPos(Vector2 pos, bool enbleTouch)
  {
    if (!enbleTouch)
    {
      OnDragEnd();
      mCurTouchPos = Vector2.zero;
      return;
    }

    mLastouchPos = mCurTouchPos;

    mCurTouchPos = pos;
    if (mLastouchPos == Vector2.zero)
    {
      OnPress(false);
      OnDragStart();
    }
    else
    {
        OnDragStart();
        OnDrag(mCurTouchPos - mLastouchPos);
    }

    
  }
  /// <summary>
  /// Start the dragging operation.
  /// </summary>
  void OnPress(bool isPressed)
  {
    OnDragDropStart(true);
  }
  void OnDragStart()
  {
    if (!enabled /*|| mTouchID != int.MinValue*/) return;
    OnDragDropStart();
  }

  /// <summary>
  /// Perform the dragging.
  /// </summary>

  void OnDrag(Vector2 delta)
  {
    if (!enabled /*|| mTouchID != UICamera.currentTouchID*/) return;
    OnDragDropMove((Vector3)delta * mRoot.pixelSizeAdjustment);
  }

  /// <summary>
  /// Notification sent when the drag event has ended.
  /// </summary>

  void OnDragEnd()
  {
    if (!enabled /*|| mTouchID != UICamera.currentTouchID*/) {
      return;
    }
    OnDragDropRelease(UICamera.hoveredObject);
  }
  #endregion

  /// <summary>
  /// Perform any logic related to starting the drag & drop operation.
  /// </summary>

  protected virtual void OnDragDropStart(bool bPress = false)
  {

    // Disable the collider so that it doesn't intercept events
    if (mCollider != null) mCollider.enabled = false;

    //mTouchID = UICamera.currentTouchID;
    //mParent = mTrans.parent;
    //mRoot = NGUITools.FindInParents<UIRoot>(mParent);

    Vector3 screenPos = new Vector3(mCurTouchPos.x, mCurTouchPos.y, 0);
    Vector3 pos = UICamera.mainCamera.ScreenToWorldPoint(screenPos);

    if(bPress)
    {
      stickBackTrans.position = pos;
      pos = stickBackTrans.localPosition;
      pos.z = 0f;
      stickBackTrans.localPosition = pos;
      mBackTarPos = pos;
      stickTrans.localPosition = pos;
      mStickTarPos = pos;
      m_TempTouchPos = pos;
      m_StickCenterPos = pos;
    }
    else
    {
      m_TempTouchPos = transform.InverseTransformPoint(pos);
      m_TempTouchPos.z = 0;
    }
    //

    if (OnJoystickStart != null) {
      OnJoystickStart(NGUIJoyStickData.JoystickAxis);
    }
    // Notify the widgets that the parent has changed
    NGUITools.MarkParentAsChanged(gameObject);
  }

  /// <summary>
  /// Adjust the dragged object's position.
  /// </summary>

  protected virtual void OnDragDropMove(Vector3 delta)
  {
    //stickTrans.localPosition += delta;
    m_TempTouchPos += delta;

    float dis = Vector3.Distance(m_TempTouchPos, m_StickCenterPos);
    bool bSmooth = false;
    if(MoveAreaRadius > 0 && dis > MoveAreaRadius)
    {
      m_StickCenterPos = Vector3.Lerp(m_TempTouchPos, m_StickCenterPos, MoveAreaRadius / dis);
      dis = MoveAreaRadius;

      mBackTarPos = m_StickCenterPos;
      bSmooth = true;
    }
    Vector3 stickPos = dis <= showAreaRadius ? m_TempTouchPos : Vector3.Lerp(m_StickCenterPos, m_TempTouchPos, showAreaRadius / dis);
    mStickTarPos = stickPos;
    if (!bSmooth && mBackTarPos == stickBackTrans.localPosition)
      stickTrans.localPosition = stickPos;

    m_StickMoveDelta.x = (m_TempTouchPos - m_StickCenterPos).x;
    m_StickMoveDelta.y = (m_TempTouchPos - m_StickCenterPos).y;
    if(Sensitive > 0 && m_StickMoveDelta.magnitude <= Sensitive)
    {
      NGUIJoyStickData.JoystickAxis = Vector2.zero;
    }
    else
      NGUIJoyStickData.JoystickAxis = m_StickMoveDelta;



    if (OnJoystickMove != null)
    {
      OnJoystickMove(NGUIJoyStickData.JoystickAxis);
    }
    CreateJoystickEvent(NGUIJoyStickEvent.OnJoystickMove, NGUIJoyStickData.JoystickAxis);
    SetAlphaValue(1);
  }

  /// <summary>
  /// Drop the item onto the specified object.
  /// </summary>

  protected virtual void OnDragDropRelease(GameObject surface)
  {
    if (!cloneOnDrag) {
      //mTouchID = int.MinValue;
      if (mCollider != null) mCollider.enabled = true;
      //重置摇杆位置
      ResetStickPos();
      CreateJoystickEvent(NGUIJoyStickEvent.OnJoystickMoveEnd,Vector2.zero);
      // Notify the widgets that the parent has changed
      NGUITools.MarkParentAsChanged(gameObject);
    } else {
      NGUITools.Destroy(gameObject);
    }
    SetAlphaValue(0.5f);
  }
  /// <summary>
  /// 获取摇杆的中心区域坐标
  /// </summary>
  private Vector3 GetStickCenterPos()
  {
    //因为是居中对齐方式，中点就是其坐标
    Vector3 pos = stickBackTrans.position;
    //转换成局部坐标
    return transform.InverseTransformPoint(pos);
  }
  /// <summary>
  /// 重置摇杆可点击区域
  /// </summary>
  private void ResetStickAreaPos()
  {
    
  }
  /// <summary>
  /// 重置摇杆的位置
  /// </summary>
  private void ResetStickPos()
  {
    if (null == stickPosTrans)
      return;
    if(stickTrans!=null) {
      stickTrans.localPosition = stickPosTrans.localPosition;
      mStickTarPos = stickTrans.localPosition;
    }
    if (stickBackTrans != null)
    {
      stickBackTrans.localPosition = stickPosTrans.localPosition;
      mBackTarPos = stickBackTrans.localPosition;
    }
    m_StickCenterPos = GetStickCenterPos();
    m_TempTouchPos = m_StickCenterPos;
    SetAlphaValue(0.5f);
  }
  /// <summary>
  /// 如果设置摇杆显示的限制区域，当点击位置大于半径时，需要重新计算显示的位置
  /// </summary>
  /// <param name="mDeltaPos"></param>
  /// <returns></returns>
  private Vector3 CalRealStickPos(float distance, Vector3 mDeltaPos)
  {
    Vector3 pos = new Vector3();
    float scale = 0.0f;
    if(distance>showAreaRadius) {
      scale = showAreaRadius / distance;
      pos.x = m_StickCenterPos.x + mDeltaPos.x * scale;
      pos.y = m_StickCenterPos.y + mDeltaPos.y * scale;
      pos.z = 0f;
      return pos;
    }
    return m_StickCenterPos;
  }
  void OnDisable()
  {
    if (TouchManager.Instance)
    {
      TouchManager.Instance.JoyStickUI = null;
    }
    ResetStickPos();
    CreateJoystickEvent(NGUIJoyStickEvent.OnJoystickMoveEnd, Vector2.zero);
  }
  void OnEnable()
  {
    TouchManager.Instance.JoyStickUI = this;

    //mTouchID = int.MinValue;
    ResetStickPos();
    if (mCollider != null) mCollider.enabled = true;
  }
  private void CreateJoystickEvent(NGUIJoyStickEvent joystickEvent,Vector2 pos)
  {
    switch(joystickEvent) {
      case NGUIJoyStickEvent.OnJoystickMove :
        if (OnJoystickMove != null) OnJoystickMove(pos);break;
      case NGUIJoyStickEvent.OnJoystickMoveEnd:
        if (OnJoystickMoveEnd != null) OnJoystickMoveEnd(pos);break;
    }
  }
  public Vector3 GetJoystickCenter()
  {
    return m_StickCenterPos;
  }
  public static void SetEnable(bool enable)
  {
    float alpha = enable?1:0.5f;
    GameObject go = UIManager.Instance.GetWindowGoByName("JoyStickPanel");
    if(go!=null) {
      NGUIJoyStick joyStick = go.GetComponentInChildren<NGUIJoyStick>();
      if(joyStick!=null) {
        UIWidget widget = joyStick.GetComponent<UIWidget>();
        if (widget != null)
          widget.alpha = alpha;
        BoxCollider collider = joyStick.GetComponent<BoxCollider>();
        if(collider!=null) {
          collider.enabled = enable;
        }
      }
    }
  }
  //设置摇杆透明度
  void SetAlphaValue(float value)
  {
    if (UIDataCache.Instance.IsNewbie())
      value = 1;
    UIPanel uiPanel = this.transform.parent.GetComponent<UIPanel>();
    if (uiPanel == null)
      return;
    uiPanel.alpha = value;
  }
}