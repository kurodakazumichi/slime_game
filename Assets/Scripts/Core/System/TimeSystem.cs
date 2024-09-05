using UnityEngine;



static public class TimeSystem
{

  static private float globalTimeScale = 1f;

  public class MyDeltaTime
  {
    public float Scale = 1f;

    public float DeltaTime {
      get { return Time.deltaTime * Scale * globalTimeScale; }
    }

    public void Pause(bool value)
    {
      Scale = (value) ? 0f : 1f;
    }
  }

  /// <summary>
  /// デフォルトで利用するデルタタイム
  /// </summary>
  static private MyDeltaTime deltaTime = new MyDeltaTime();

  static public float DeltaTime {
    get { return deltaTime.DeltaTime; }
  }

  static public bool Pause {
    set { deltaTime.Scale = value ? 0f : 1f; }
  }

  static private MyDeltaTime player = new MyDeltaTime();
  static public MyDeltaTime Player { get { return player; } }

  static private MyDeltaTime enemy = new MyDeltaTime();
  static public MyDeltaTime Enemy {  get { return enemy; } }

  static private MyDeltaTime _wave = new MyDeltaTime();
  static public MyDeltaTime Wave { get { return _wave; } }

  /// <summary>
  /// シーンフロー制御用の時間
  /// </summary>
  static private MyDeltaTime _scene = new MyDeltaTime();
  static public MyDeltaTime Scene {
    get { return _scene; }
  }

  /// <summary>
  /// UI制御用の時間
  /// </summary>
  static private MyDeltaTime _ui = new MyDeltaTime();
  static public MyDeltaTime UI {
    get { return _ui; }
  }

  /// <summary>
  /// スキル発動を制御する時間
  /// </summary>
  static private MyDeltaTime _skill = new MyDeltaTime();
  static public MyDeltaTime Skill {
    get { return _skill; }
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  static public void OnDebug()
  {
    using (new GUILayout.VerticalScope(GUI.skin.box)) 
    {
      GUILayout.Label("TimeSystem");

      using (new GUILayout.HorizontalScope()) {
        GUILayout.Label("GlobalTimeScale");
        GUILayout.TextField(globalTimeScale.ToString(), GUILayout.Width(30));
        if (GUILayout.Button("Reset")) {
          globalTimeScale = 1f;
        }
      }
      globalTimeScale = GUILayout.HorizontalSlider(globalTimeScale, 0f, 10f);

      OnDebugDeltaTime("DefaultTimeScale", deltaTime);
      OnDebugDeltaTime("PlayerTimeScale", player);
      OnDebugDeltaTime("EnemyTimeScale", enemy);
    }
  }

  private static void OnDebugDeltaTime(string title, MyDeltaTime delta)
  {
    using (new GUILayout.HorizontalScope()) {
      GUILayout.Label(title);
      GUILayout.TextField(delta.Scale.ToString(), GUILayout.Width(30));
      if (GUILayout.Button("Reset")) {
        delta.Scale = 1f;
      }
    }
    delta.Scale = GUILayout.HorizontalSlider(delta.Scale, 0f, 10f);
  }

#endif


}
