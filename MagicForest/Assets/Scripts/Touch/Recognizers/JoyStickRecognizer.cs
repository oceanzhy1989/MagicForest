using UnityEngine;
using System.Collections;

public class JoyStickGesture : Gesture {
 
  bool down = false;
  bool wasdown = false;

  public TouchManager.Finger lockedFinger = null;
  public bool Down
  {
    get
    {
      return down;
    }
    set
    {
      down = value;
    }
  }

  public bool WasDown
  {
    get
    {
      return wasdown;
    }
    set
    {
      wasdown = value;
    }
  }
}

public class JoyStickRecognizer : GestureRecognizerBase<JoyStickGesture> {
  public int RequiredTaps = 1;
  /// 手势允许移动的距离
  public float MoveTolerance = 0.5f;

  protected override void Reset(JoyStickGesture gesture, bool isPressed = false)
  {
    gesture.Down = false;
    gesture.WasDown = false;
    base.Reset(gesture);
  }

  GestureRecognitionState RecognizeJoyStick(JoyStickGesture gesture, TouchManager.IFingerList touches)
  {
    if (!gesture.lockedFinger.IsDown || UICamera.isOverUI || TouchManager.Instance.JoyStickUI == null)
    {
      if (TouchManager.Instance.JoyStickUI)
      {
        TouchManager.Instance.JoyStickUI.SetTouchPos(Vector2.zero, false);
      }

      gesture.lockedFinger.TryUnLockFinger(this);
      gesture.lockedFinger = null;
      return GestureRecognitionState.Failed;
    }

    gesture.Position = gesture.lockedFinger.Position;

    TouchManager.Instance.JoyStickUI.SetTouchPos(gesture.Position, true);

    return GestureRecognitionState.InProgress;
  }

  public override string GetDefaultEventMessageName()
  {
    return string.IsNullOrEmpty(EventMessageName) ? "OnJoyStick" : EventMessageName;
  }

  protected override bool CanBegin(JoyStickGesture gesture, TouchManager.IFingerList touches)
  {
    if (touches.Count <= 0 || TouchManager.Instance.JoyStickUI == null)
    {
      return false;
    }

    if ("MouseInput" == TouchManager.Instance.InputProvider.name)
    {
      if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) < 0.0001f)
      {
        return false;
      }
    }

    if (MagicForestClient.TouchType.Regognizer != TouchManager.curTouchState)
    {
      return false;
    }

    if (UICamera.isOverUI)
    {
      return false;
    }


    for (int i = 0; i < touches.Count; i++)
    {
      TouchManager.Finger fr = touches[i];
      if (fr.IsMoving)
        continue;

      if (TouchManager.Instance.JoyStickUI.IsPosInJoyStick(fr.StartPosition))
      {
        gesture.lockedFinger = fr;
        return fr.TryLockFinger(this);
      }
    }

    return false;
  }

  protected override void OnBegin(JoyStickGesture gesture, TouchManager.IFingerList touches)
  {
    gesture.Position = gesture.lockedFinger.Position;
    gesture.StartPosition = gesture.Position;
  }

  protected override GestureRecognitionState OnRecognize(JoyStickGesture gesture, TouchManager.IFingerList touches)
  {
    return RecognizeJoyStick(gesture, touches);
  }
}
