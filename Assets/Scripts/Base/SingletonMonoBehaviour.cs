using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MyMonoBehaviour where T : MyMonoBehaviour
{
  /// <summary>
  /// Singleton Instance
  /// </summary>
  private static T instance;

  /// <summary>
  /// Instanceのアクセッサ
  /// </summary>
  public static T Instance {
    get {
      if (instance == null) {
        instance = (T)FindFirstObjectByType(typeof(T));
        if (instance == null) {
          Debug.LogError(typeof(T) + "がシーンに存在しません。");
        }
      }
      return instance;
    }
  }

  /// <summary>
  /// インスタンスがあるかどうか
  /// </summary>
  public static bool HasInstance => (instance != null);

  /// <summary>
  /// 2つ以上のインスタンスが生成された場合は、破棄して終了する。
  /// </summary>
  protected override void MyAwake() 
  {
    Logger.Log($"[SingletonMonoBehaviour] MyAwake() {gameObject.name}");

    if (this != Instance) {
      Logger.Warn($"{typeof(T).Name} が1回以上生成されるフローが存在します。");
      Destroy(this);
      return;
    }

    base.MyAwake();
  }
}
