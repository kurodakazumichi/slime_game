using UnityEngine;

static public class CollisionUtil
{
  /// <summary>
  /// 2“_ŠÔ‚Ì‹——£‚ÌÕ“Ë”»’è
  /// </summary>
  static public bool IsCollideAxB(Vector3 a, Vector3 b, float distance)
  {
    return (a - b).sqrMagnitude < distance * distance;
  }
}
