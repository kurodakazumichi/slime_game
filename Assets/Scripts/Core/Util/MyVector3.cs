using UnityEngine;

public static class MyVector3
{
  public static Vector3 Random(float x, float y, float z)
  {
    return new Vector3(
      UnityEngine.Random.Range(-x, x),
      UnityEngine.Random.Range(-y, y),
      UnityEngine.Random.Range(-z, z)
    );
  }

  /// <summary>
  /// originベクトルをaxisベクトルを軸としてランダムに回転したベクトルを返す
  /// </summary>
  public static Vector3 Random(Vector3 origin, Vector3 axis)
  {
    return Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), axis) * origin;
  }

  /// <summary>
  /// 指定したベクトルaがdistanceより大きいならばtrue
  /// </summary>
  public static bool IsOverDistance(Vector3 a, float distance)
  {
    return (distance*distance) < a.sqrMagnitude;
  }

  /// <summary>
  /// ベクトルの一番大きい成分
  /// </summary>
  public static float LargestCompnent(Vector3 a)
  {
    return Mathf.Max(Mathf.Max(a.x, a.y), a.z);
  }
}
