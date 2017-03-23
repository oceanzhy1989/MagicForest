using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UIPanelInspectorEx {
  public enum PanelDepthQueue
  {
    UIRoot_0 = -10,
    Background_01 = 0,
    FullScreen_02 = 10,
    Transparent_03 = 50,
    Dialog_04 = 100,
    Overlay_05= 150
  }
  static public PanelDepthQueue GetPanelDepthQueue(int depth)
  {
    if (depth >= (int)PanelDepthQueue.Overlay_05)
      return PanelDepthQueue.Overlay_05;
    if (depth >= (int)PanelDepthQueue.Dialog_04)
      return PanelDepthQueue.Dialog_04;
    if (depth >= (int)PanelDepthQueue.Transparent_03)
      return PanelDepthQueue.Transparent_03;
    if (depth >= (int)PanelDepthQueue.FullScreen_02)
      return PanelDepthQueue.FullScreen_02;
    if (depth >= (int)PanelDepthQueue.Background_01)
      return PanelDepthQueue.Background_01;
    return PanelDepthQueue.UIRoot_0;
  }
  static public int GetFixedDepth(PanelDepthQueue queue,int depth)
  {
    //because of UIPanel.PanelDepthQueue.UIRoot_0 = -10;
    //see also :Enum.GetValues(Type type);
    if (queue == PanelDepthQueue.UIRoot_0) {
      if (depth < (int)queue)
        return (int)queue;
      if (depth >= (int)PanelDepthQueue.Background_01)
        return (int)PanelDepthQueue.Background_01-1;
      return depth;
    }
    Array enums = Enum.GetValues(typeof(PanelDepthQueue));
    for (int index = 0; index < enums.Length-2; ++index) {
      if ((int)queue == (int)enums.GetValue(index)) {
        if (depth >= (int)enums.GetValue(index + 1))
          return (int)enums.GetValue(index + 1)-1;
      }
    }
    if (depth<(int)queue) {
      return (int)queue;
    }
    return depth;
  }
}
