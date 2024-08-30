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
  /// �f�t�H���g�ŗ��p����f���^�^�C��
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
  /// �V�[���t���[����p�̎���
  /// </summary>
  static private MyDeltaTime _scene = new MyDeltaTime();
  static public MyDeltaTime Scene {
    get { return _scene; }
  }

  /// <summary>
  /// UI����p�̎���
  /// </summary>
  static private MyDeltaTime _ui = new MyDeltaTime();
  static public MyDeltaTime UI {
    get { return _ui; }
  }

  /// <summary>
  /// �X�L�������𐧌䂷�鎞��
  /// </summary>
  static private MyDeltaTime _skill = new MyDeltaTime();
  static public MyDeltaTime Skill {
    get { return _skill; }
  }


}
