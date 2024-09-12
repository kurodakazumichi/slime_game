using UnityEngine;

public static class MyVector3
{
  public static Vector3 Random(Vector3 area)
  {
    return new Vector3(
      UnityEngine.Random.Range(-area.x, area.x),
      UnityEngine.Random.Range(-area.y, area.y),
      UnityEngine.Random.Range(-area.z, area.z)
    );
  }

  /// <summary>
  /// 指定したベクトルaがdistanceより大きいならばtrue
  /// </summary>
  public static bool IsOverDistance(Vector3 a, float distance)
  {
    return (distance*distance) < a.sqrMagnitude;
  }
}
