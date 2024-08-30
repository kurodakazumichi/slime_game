using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugManager : SingletonMonoBehaviour<DebugManager>
{
  //============================================================================
  // Variables
  //============================================================================

#if _DEBUG
  /// <summary>
  /// DebugManagerの管理対象になるMyMonoBehaviour
  /// </summary>
  private Dictionary<string, MyMonoBehaviour> monos 
      = new Dictionary<string, MyMonoBehaviour>();

  /// <summary>
  /// 現在デバッグ対象になっているMyMonoBehaivour
  /// </summary>
  private MyMonoBehaviour current = null;

  /// <summary>
  /// DebugWindowを表示するかどうかのフラグ
  /// </summary>
  private bool isShow = false;

  /// <summary>
  /// スクロール量
  /// </summary>
  private Vector2 scrollPosition = Vector2.zero;
#endif

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  /// <summary>
  /// デバッグマネージャーに対称を登録
  /// </summary>
  /// <param name="mono"></param>
  [Conditional("_DEBUG")]
  public void Regist(MyMonoBehaviour mono)
  {
#if _DEBUG
    this.monos.Add(mono.GetType().Name, mono);
#endif
  }

  /// <summary>
  /// デバッグマネージャーから対象を破棄
  /// </summary>
  [Conditional("_DEBUG")]
  public void Discard(MyMonoBehaviour mono)
  {
#if _DEBUG
    string name = mono.GetType().Name;

    if (this.monos.ContainsKey(mono.GetType().Name)) {
      this.monos.Remove(name);
    }
#endif
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

#if _DEBUG
  void Update()
  {
    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) {
      this.isShow = !isShow;
    }
  }

  private void OnGUI()
  {
    if (!isShow) return;

    using (var sv = new GUILayout.ScrollViewScope(scrollPosition)) {

      using (new GUILayout.HorizontalScope(GUI.skin.box)) {
        scrollPosition = sv.scrollPosition;

        using (new GUILayout.VerticalScope(GUILayout.Width(100))) 
        {
          foreach(var mono in monos) {
            if (GUILayout.Button(mono.Key)) {
              this.current = mono.Value;
            }
          }
        }
        
        using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(Screen.width - 150))) 
        {
          if (current) {
            current.OnDebug();
          }
        }
      }
    }
  }
#endif

}