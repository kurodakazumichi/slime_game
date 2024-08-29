using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLocation : MonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  private Dictionary<int, List<EnemyWaveParam>> waveData = new Dictionary<int, List<EnemyWaveParam>>();

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// Waveデータを持っているか
  /// </summary>
  public bool HasWaves {
    get {
      return 0 < waveData.Count;
    }
  }

  /// <summary>
  /// 出現する敵のIDリスト
  /// </summary>
  public List<EnemyId> AppearingEnemyIds 
  {
    get { 
      var list = new List<EnemyId>();

      ForeachWaveData((wave) => {
        if (!list.Contains(wave.Id)) {
          list.Add(wave.Id);
        }
      });

      return list;
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Unity Life Cycle
  //----------------------------------------------------------------------------

  void Awake()
  {
    CollectWaveData();
  }

  //----------------------------------------------------------------------------
  // for Me
  //----------------------------------------------------------------------------

  /// <summary>
  /// BattleLocationに含まれるEnemyWaveParamをかき集める
  /// </summary>
  private void CollectWaveData()
  {
    // WaveDataという名前のオブジェクトを探す
    var waveDataObject = transform.Find("WaveData");

    if (waveDataObject == null) {
      Logger.Error("[BattleLocation] This BattleLocation does not have WaveData Object.");
      return;
    }

    // WaveDataオブジェクト配下にあるWave0, Wave1, ... という名前のオブジェクトを探す
    for (int i = 0; true; i++) 
    {
      var wave = waveDataObject.Find($"Wave{i}");

      if (wave == null) {
        return;
      }

      // WaveXというオブジェクトの配下にあるEnemyWaveSettingsコンポーネントを全取得
      var settings = wave.GetComponentsInChildren<EnemyWaveSettings>();

      // 設定からEnemyWaveParamインスタンスを生成して保持
      foreach (var setting in settings) 
      {
        var param = EnemyWaveParam.Make(setting);
        AddWaveParam(i, param);
      }
    }
  }

  /// <summary>
  /// WaveDataにEnemyWaveParamを追加する
  /// </summary>
  private void AddWaveParam(int no, EnemyWaveParam param)
  {
    if (waveData.TryGetValue(no, out var waves)) 
    {
      waves.Add(param);
    } else {
      waveData.Add(no, new List<EnemyWaveParam> { param });
    }
  }

  /// <summary>
  /// WaveDataに含まれるEnemyWaveParamの数だけループをする
  /// </summary>
  private void ForeachWaveData(Action<EnemyWaveParam> action)
  {
    foreach (var waves in waveData.Values) 
    {
      if (waves == null) continue;

      foreach (var wave in waves) 
      {
        if (wave != null) {
          action(wave);
        }
      }
    }
  }
}
