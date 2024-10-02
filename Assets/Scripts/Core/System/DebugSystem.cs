using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MyGame.Core.System
{
  public interface IDebugable
  {
    string GetName() {
      return GetType().Name;
    }
    void OnDebug();
  }

  public static class DebugSystem
  {
    //============================================================================
    // Variables
    //============================================================================

    /// <summary>
    /// DebugSystemの管理対象になるクラスインスタンス群
    /// </summary>
    private static Dictionary<string, IDebugable> debugs = new();

    /// <summary>
    /// 現在デバッグ対象になっているクラスインスタンス
    /// </summary>
    private static IDebugable current;

    /// <summary>
    /// DebugWindowを表示するかどうかのフラグ
    /// </summary>
    private static bool isShow = false;

    /// <summary>
    /// スクロール量
    /// </summary>
    private static Vector2 scrollPosition = Vector2.zero;

    /// <summary>
    /// 非ピクセルパーフェクトなGUIのために画面サイズを定義しておく
    /// </summary>
    private static Vector2 screenSize = new Vector2(768, 432);

    /// <summary>
    /// デバッグマネージャーに対称を登録
    /// </summary>
    [Conditional("_DEBUG")]
    public static void Regist(IDebugable debugger)
    {
      var name = debugger.GetName();
      debugs.Add(name, debugger);
    }

    /// <summary>
    /// デバッグマネージャーから対象を破棄
    /// </summary>
    [Conditional("_DEBUG")]
    public static void Discard(IDebugable debugger)
    {
      var name = debugger.GetType().Name;

      if (debugs.ContainsKey(name)) {
        debugs.Remove(name);
      }
    }

    [Conditional("_DEBUG")]
    public static void Update()
    {
      if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) {
        isShow = !isShow;
      }
    }

    [Conditional("_DEBUG")]
    public static void OnGUI()
    {
      if (!isShow) return;

      // GUI用の解像度を更新
      GUIUtility.ScaleAroundPivot(new Vector2(Screen.width / screenSize.x, Screen.height / screenSize.y), Vector2.zero);

      using (var sv = new GUILayout.ScrollViewScope(scrollPosition, GUILayout.Width(screenSize.x), GUILayout.Height(screenSize.y))) {

        using (new GUILayout.HorizontalScope(GUI.skin.box)) {
          scrollPosition = sv.scrollPosition;

          using (new GUILayout.VerticalScope(GUILayout.Width(100))) 
          {
            foreach(var debugger in debugs) {
              if (GUILayout.Button(debugger.Key)) {
                current = debugger.Value;
              }
            }
          }
        
          using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(screenSize.x - 150))) 
          {
            if (current != null) {
              current.OnDebug();
            }
          }
        }
      }

      GUI.matrix = Matrix4x4.identity;
    }
  }
}