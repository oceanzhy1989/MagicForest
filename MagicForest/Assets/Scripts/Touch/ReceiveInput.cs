using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MagicForestClient;


public class ReceiveInput : MonoBehaviour
{
  internal void Start()
  {
    try {
      ///
      TouchManager.OnGestureHintEvent += OnGestureHintEvent;
      TouchManager.OnGestureEvent += OnGestureEvent;
      TouchManager.OnFingerEvent += OnFingerEvent;
      ///
     
      NGUIJoyStick.OnJoystickMove += On_NGUIJoystickMove;
      NGUIJoyStick.OnJoystickMoveEnd += On_NGUIJoystickMoveEnd;
    } catch (System.Exception ex) {
      //DashFire.LogicForGfxThread.LogicErrorLog("Exception {0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  private void SetTouchEnable(bool value)
  {
    try {
      TouchManager.TouchEnable = value;
    } catch (Exception ex) {
      //DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  private void SetJoystickEnable(bool value)
  {
    try {
      JoyStickInputProvider.JoyStickEnable = value;
    } catch (Exception ex) {
      //DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  internal void Update()
  {

  }

  internal void OnDestroy()
  {
    TouchManager.OnGestureHintEvent -= OnGestureHintEvent;
    TouchManager.OnGestureEvent -= OnGestureEvent;
    TouchManager.OnFingerEvent -= OnFingerEvent;
    ///
    //GfxModule.Skill.Trigers.TriggerUtil.IsWantMoving = false;
  }

  private void OnGestureHintEvent(Gesture gesture)
  {
    if (null == gesture) {
      return;
    }
  }

  private void OnGestureEvent(Gesture gesture)
  {
    if (null == gesture) {
      return;
    }
    Vector3 target_pos = gesture.GetStartTouchToWorldPoint();
    if (null != gesture.Recognizer && Vector3.zero != target_pos) {
      if ("OnDoubleTap" == gesture.Recognizer.EventMessageName
        && gesture.SelectedID < 0) {
        //GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(DashFire.LogicForGfxThread.PlayerSelf);
        //GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(DashFire.LogicForGfxThread.PlayerSelf, SkillCategory.kRoll, target_pos);
        return;
      }
    }
    /*
    if (SkillCategory.kNone != gesture.SkillTags) {
      switch (gesture.SkillTags) {
        case SkillCategory.kSkillA:
        case SkillCategory.kSkillB:
        case SkillCategory.kSkillC:
        case SkillCategory.kSkillD:
        case SkillCategory.kSkillQ:
        case SkillCategory.kSkillE:
          GfxModule.Skill.GfxSkillSystem.Instance.StopAttack(DashFire.LogicForGfxThread.PlayerSelf);
          if (gesture.SectionNum > 0) {
            if (waite_skill_buffer.Count > 0) {
              CandidateSkillInfo candidate_skill_info = new CandidateSkillInfo();
              candidate_skill_info.skillType = gesture.SkillTags;
              candidate_skill_info.targetPos = Vector3.zero;
              waite_skill_buffer.Add(candidate_skill_info);
            } else {
              GfxModule.Skill.GfxSkillSystem.Instance.PushSkill(DashFire.LogicForGfxThread.PlayerSelf, gesture.SkillTags, Vector3.zero);
            }
          } else {
            waite_skill_buffer.Clear();
            CandidateSkillInfo candidate_skill_info = new CandidateSkillInfo();
            candidate_skill_info.skillType = gesture.SkillTags;
            candidate_skill_info.targetPos = target_pos;
            waite_skill_buffer.Add(candidate_skill_info);
            GestureArgs e = TouchManager.ToGestureArgs(gesture);
            LogicForGfxThread.FireGestureEvent(e);
          }
          break;
      }
    }
    */
  }

  private void OnFingerEvent(FingerEvent fevent)
  {
    if (TouchManager.GestureEnable) {
      if (fevent.Finger.IsDown && !fevent.Finger.WasDown) {
        //DashFire.LogicForGfxThread.EventForGfx.Publish("ge_finger_event", "ui", fevent.Position, true);
      } else if (fevent.Finger.WasDown && !fevent.Finger.IsDown) {
        //DashFire.LogicForGfxThread.EventForGfx.Publish("ge_finger_event", "ui", fevent.Position, false);
      }
    }
  }

  //≤‚ ‘
  void On_NGUIJoystickMoveEnd(Vector2 move)
  {
    TriggerMove(move, true);
  }
  //≤‚ ‘
  void On_NGUIJoystickMove(Vector2 joystickAlix)
  {
    if (TouchManager.Touches.Count > 0) {
      TriggerMove(joystickAlix, false);
    }
  }
  //≤‚ ‘
  private void TriggerMove(Vector2 move, bool isLift)
  {
    if (isLift || move == Vector2.zero) {
      GestureArgs e = new GestureArgs();
      e.name = "OnSingleTap";
      e.airWelGamePosX = 0f;
      e.airWelGamePosY = 0f;
      e.airWelGamePosZ = 0f;
      e.selectedObjID = -1;
      e.towards = -1f;
      e.inputType = InputType.Joystick;
      //LogicForGfxThread.FireGestureEvent(e);
      EventDispatcher.Instance.SetJoystickInfo(e);
      return;
    }

    GameObject playerSelf = LogicInterface.Instance.PlayerSelf;
    if (playerSelf != null) {
            playerSelf.name = "MainChar0";
      Vector2 joyStickDir = move;
      Vector3 targetRot = new Vector3(joyStickDir.x, 0, joyStickDir.y);
      Vector3 targetPos = playerSelf.transform.position + targetRot;

      GestureArgs e = new GestureArgs();
      e.name = "OnSingleTap";
      e.selectedObjID = -1;
      float towards = Mathf.Atan2(joyStickDir.x, joyStickDir.y);
      e.towards = towards;
      e.airWelGamePosX = targetPos.x;
      e.airWelGamePosY = targetPos.y;
      e.airWelGamePosZ = targetPos.z;
      e.inputType = InputType.Joystick;
      //LogicForGfxThread.FireGestureEvent(e);
      EventDispatcher.Instance.SetJoystickInfo(e);
    }
  }

  /// effect handle
  //private void EffectHandle(GestureEvent ge, float posX, float posY, float posZ, bool isSelected, bool isLogicCmd)
  //{
  //  try {
  //    if (DFMUiRoot.InputMode == InputType.Joystick) {
  //      return;
  //    }
  //    Vector3 effect_pos = new Vector3(posX, posY, posZ);
  //    if (GestureEvent.OnSingleTap == ge) {
  //      if (isSelected) {
  //        if (!isLogicCmd) {
  //          PlayEffect(Go_Lock, effect_pos, Go_Lock_Time);
  //        }
  //      } else {
  //        PlayEffect(Go_Landmark, effect_pos, Go_Landmark_Time);
  //        HideSkillTip();
  //      }
  //    }
  //  } catch (Exception ex) {
  //    DashFire.LogicForGfxThread.LogicLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
  //  }
  //}

  //private void PlayEffect(GameObject effectPrefab, Vector3 position, float playTime)
  //{
  //  GameObject obj = DashFire.ResourceSystem.NewObject(effectPrefab, playTime) as GameObject;
  //  if (obj != null) {
  //    obj.transform.position = position;
  //    obj.transform.rotation = Quaternion.identity;
  //  }
  //}

  protected bool can_conjure_q_skill = false;
  protected bool can_conjure_e_skill = false;
  protected bool isRegister = false;
  protected GameObject m_SkillTipObj = null;
  public GameObject Go_Landmark = null;
  public float Go_Landmark_Time = 0.2f;
  public GameObject Go_Lock = null;
  public GameObject SkillTipPrefab = null;
  public float Go_Lock_Time = 0.2f;
  ///
  public delegate void EventHandler(float towards);
  public static EventHandler OnJoystickMove;
}