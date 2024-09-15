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
  /// シーンとUI意外をポーズ
  /// </summary>
  static public bool MenuPause {
    set {
      Player.Pause(value);
      Enemy.Pause(value);
      Wave.Pause(value);
      Skill.Pause(value);
      Bullet.Pause(value);
    }
  }

  /// <summary>
  /// シーンフロー制御用
  /// </summary>
  static public MyDeltaTime Scene { get; private set; } = new();

  /// <summary>
  /// UI制御用
  /// </summary>
  static public MyDeltaTime UI { get; private set; } = new();

  /// <summary>
  /// プレイヤー制御用
  /// </summary>
  static public MyDeltaTime Player { get; private set; } = new();

  /// <summary>
  /// 敵制御用
  /// </summary>
  static public MyDeltaTime Enemy { get; private set; } = new();

  /// <summary>
  /// Wave制御用
  /// </summary>
  static public MyDeltaTime Wave { get; private set; } = new();

  /// <summary>
  /// Bullet制御用
  /// </summary>
  static public MyDeltaTime Bullet { get; private set; } = new();

  /// <summary>
  /// スキル発動の制御用
  /// </summary>
  static public MyDeltaTime Skill { get; private set; } = new();

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

      OnDebugDeltaTime("SceneTimeScale", Scene);
      OnDebugDeltaTime("UITimeScale", UI);
      OnDebugDeltaTime("PlayerTimeScale", Player);
      OnDebugDeltaTime("EnemyTimeScale", Enemy);
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
