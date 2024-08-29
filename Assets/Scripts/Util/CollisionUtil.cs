using UnityEngine;

static public class CollisionUtil
{
  /// <summary>
  /// 2�_�Ԃ̋����̏Փ˔���
  /// </summary>
  static public bool IsCollideAxB(Vector3 a, Vector3 b, float distance)
  {
    return (a - b).sqrMagnitude < distance * distance;
  }
}
