using System.Diagnostics;

/// <summary>
/// Debug.Log�n�̃��b�p�[�N���X
/// �}�N����`�ɂ�胁�\�b�h�Ăяo���̗L���������\
/// </summary>
public static class Logger
{
  [Conditional("_DEBUG")]
  public static void Log(object message)
  {
    UnityEngine.Debug.Log(message);
  }

  [Conditional("_DEBUG")]
  public static void Error(object message)
  {
    UnityEngine.Debug.LogError(message);
  }

  [Conditional("_DEBUG")]
  public static void Warn(object message)
  {
    UnityEngine.Debug.LogWarning(message);
  }
}
