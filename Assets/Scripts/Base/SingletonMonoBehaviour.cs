using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
  /// <summary>
  /// Singleton Instance
  /// </summary>
  private static T instance;

  /// <summary>
  /// Instance�̃A�N�Z�b�T
  /// </summary>
  public static T Instance {
    get {
      if (instance == null) {
        instance = (T)FindFirstObjectByType(typeof(T));
        if (instance == null) {
          Debug.LogError(typeof(T) + "���V�[���ɑ��݂��܂���B");
        }
      }
      return instance;
    }
  }

  /// <summary>
  /// �C���X�^���X�����邩�ǂ���
  /// </summary>
  public static bool HasInstance => (instance != null);

  /// <summary>
  /// 2�ȏ�̃C���X�^���X���������ꂽ�ꍇ�́A�j�����ďI������B
  /// </summary>
  void Awake()
  {
    Debug.Log($"Done Awake Singleton of {gameObject.name}");
    if (this != Instance) {
      Debug.LogWarning($"{typeof(T).Name} ��1��ȏ㐶�������t���[�����݂��܂��B");
      Destroy(this);
      return;
    }

    MyAwake();
  }

  protected virtual void MyAwake() { }
}
