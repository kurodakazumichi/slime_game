using UnityEngine;



static public class TimeSystem
{

  static private float _globalTimeScale = 1f;

  public class MyDeltaTime
  {
    public float Scale = 1f;

    public float DeltaTime {
      get { return Time.deltaTime * Scale * _globalTimeScale; }
    }

    public void Pause(bool value)
    {
      Scale = (value) ? 0f : 1f;
    }
  }

  /// <summary>
  /// デフォルトで利用するデルタタイム
  /// </summary>
  static private MyDeltaTime _deltaTime = new MyDeltaTime();

  static public float DeltaTime {
    get { return _deltaTime.DeltaTime; }
  }

  static public bool Pause {
    set { _deltaTime.Scale = value ? 0f : 1f; }
  }

  static private MyDeltaTime player = new MyDeltaTime();
  static public MyDeltaTime Player { get { return player; } }

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
        GUILayout.TextField(_globalTimeScale.ToString(), GUILayout.Width(30));
        if (GUILayout.Button("Reset")) {
          _globalTimeScale = 1f;
        }
      }
      _globalTimeScale = GUILayout.HorizontalSlider(_globalTimeScale, 0f, 10f);

      using (new GUILayout.HorizontalScope()) {
        GUILayout.Label("DefaultTimeScale");
        GUILayout.TextField(_deltaTime.Scale.ToString(), GUILayout.Width(30));
        if (GUILayout.Button("Reset")) {
          _deltaTime.Scale = 1f;
        }
      }
      _deltaTime.Scale = GUILayout.HorizontalSlider(_deltaTime.Scale, 0f, 10f);

      using (new GUILayout.HorizontalScope()) {
        GUILayout.Label("PlayerTimeScale");
        GUILayout.TextField(player.Scale.ToString(), GUILayout.Width(30));
        if (GUILayout.Button("Reset")) {
          player.Scale = 1f;
        }
      }
      player.Scale = GUILayout.HorizontalSlider(player.Scale, 0f, 10f);
    }
  }

#endif


}
