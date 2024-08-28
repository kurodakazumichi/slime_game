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


}
