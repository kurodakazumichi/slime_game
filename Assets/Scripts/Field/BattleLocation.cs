using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLocation : MonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// WaveSettings
  /// </summary>
  private Dictionary<int, List<EnemyWaveSettings>> settings = new Dictionary<int, List<EnemyWaveSettings>>();

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// Waveデータを持っているか
  /// </summary>
  public bool HasWaves {
    get {
      return 0 < settings.Count;
    }
  }

  /// <summary>
  /// 出現する敵のIDリスト
  /// </summary>
  public List<EnemyId> AppearingEnemyIds 
  {
    get { 
      var list = new List<EnemyId>();

      ForeachWaveData((setting) => 
      {
        var id = MyEnum.Parse<EnemyId>(setting.EnemyId);

        if (!list.Contains(id)) {
          list.Add(id);
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

      // WaveXというオブジェクトの配下にあるEnemyWaveSettingsコンポーネントを取得、保持
      var settings = wave.GetComponentsInChildren<EnemyWaveSettings>();
      this.settings.Add(i, new List<EnemyWaveSettings>(settings));
    }
  }

  /// <summary>
  /// WaveDataに含まれるEnemyWaveParamの数だけループをする
  /// </summary>
  private void ForeachWaveData(Action<EnemyWaveSettings> action)
  {
    foreach (var settings in settings.Values) 
    {
      if (settings == null) continue;

      foreach (var setting in settings) 
      {
        if (setting != null) {
          action(setting);
        }
      }
    }
  }
}
