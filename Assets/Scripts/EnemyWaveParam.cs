using UnityEngine;

/// <summary>
/// 敵Waveパラメータ、Waveの形状や出現する敵の数や位置などの情報を保持する
/// </summary>
public class EnemyWaveParam
{
  /// <summary>
  /// 出現する敵ID
  /// </summary>
  public EnemyId Id { get; set; }

  /// <summary>
  /// Waveの形状
  /// </summary>
  public WaveShape Shape { get; set; }

  /// <summary>
  /// Waveの起点
  /// </summary>
  public Vector3 BasePosition { get; set; }

  /// <summary>
  /// Waveが敵を生成するエリア
  /// </summary>
  public Vector3 Area { get; set; }

  /// <summary>
  /// Wave数
  /// </summary>
  public int WaveCount { get; set; }

  /// <summary>
  /// 次のWaveが発動するまでの間隔
  /// </summary>
  public float WaveInterval { get; set; }

  /// <summary>
  /// 1つのWaveが生成する敵の数
  /// </summary>
  public int EnemyAmountPerWave { get; set; }

  /// <summary>
  /// 初回Waveが発動するまでの待機時間
  /// </summary>
  public float WaitTime { get; set; } = 0;

  /// <summary>
  /// Waveが生成する敵の合計数
  /// </summary>
  public int TotalEnemyCount {
    get { return WaveCount * EnemyAmountPerWave; }
  }

  /// <summary>
  /// 起点となる角度、このパラメータはShape.Circleのときのみ有効
  /// </summary>
  public float OriginAngle { get; set; }

  /// <summary>
  /// Wave毎に適用される角度のオフセット
  /// </summary>
  public float WaveOffsetAngle { get; set; }

  /// <summary>
  /// Wave毎に適用されるX方向オフセット
  /// </summary>
  public float WaveOffsetX {
    get { return waveOffset.x; }
    set { waveOffset.x = value; }
  }

  /// <summary>
  /// Wave毎に適用されるY方向オフセット
  /// </summary>
  public float WaveOffsetY {
    get { return waveOffset.y; }
    set { waveOffset.y = value; }
  }

  /// <summary>
  /// Wave毎に適用されるZ方向オフセット
  /// </summary>
  public float WaveOffsetZ {
    get { return waveOffset.z; }
    set { waveOffset.z = value; }
  }

  /// <summary>
  /// Wave毎に適用されるオフセット、zは角度に使う
  /// </summary>
  private Vector3 waveOffset;

  /// <summary>
  /// 出現位置のX方向を反転する
  /// このパラメーターはSphae.Lineのときのみ有効
  /// </summary>
  public bool InverseX { get; set; }

  /// <summary>
  /// 出現位置のY方向を反転する
  /// このパラメーターはSphae.Lineのときのみ有効
  /// </summary>
  public bool InverseY { get; set; }

  /// <summary>
  /// 出現位置のZ方向を反転する
  /// このパラメーターはSphae.Lineのときのみ有効
  /// </summary>
  public bool InverseZ { get; set; }
}
