using UnityEngine;

public static class MyMath
{
  /// <summary>
  /// 小数点の桁数を指定して丸める
  /// </summary>
  public static float Round(float num, int digit)
  {
    float pow = Mathf.Pow(10, digit);
    return Mathf.Floor(num * pow) / pow;
  }

  /// <summary>
  /// 補間
  /// </summary>
  public static float Lerp(float from, float to, float rate)
  {
    return from + (to - from) * rate;
  }

  /// <summary>
  /// 度を弧度に変換
  /// </summary>
  public static float Deg2Rad(float deg)
  {
    return Mathf.Deg2Rad * deg;
  }

  /// <summary>
  /// 弧度を度に変換
  /// </summary>
  public static float Rad2Deg(float rad)
  {
    return Mathf.Rad2Deg * rad;
  }

  /// <summary>
  /// 割合を弧度に変換
  /// 0は0度、1.0は360度を表すラジアン角を返す
  /// </summary>
  public static float Rate2Rad(float rate)
  {
    return Mathf.PI * 2f * rate;
  }
}
