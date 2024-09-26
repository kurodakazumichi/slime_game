﻿using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class SkillUtil
{
  /// <summary>
  /// 設定の基づいてexpからLvを逆算する
  /// </summary>
  public static int CalcLevelBy(ISkillEntityRO config, int exp)
  {
    for (int i = App.SKILL_MAX_LEVEL; 0 <= i; --i) {

      if (GetNeedExp(config, i) <= exp) {
        return i;
      }
    }

    return 0;
  }

  /// <summary>
  /// Lvに必要な経験値を取得
  /// </summary>
  public static int GetNeedExp(ISkillEntityRO config, int lv)
  {
    // 成長タイプ別係数
    const float GROWTH_FAST_FACTOR = 2.0f;
    const float GROWTH_SLOW_FACTOR = 0.5f;

    lv = Mathf.Clamp(lv, 0, App.SKILL_MAX_LEVEL);

    var rate = (float)(lv) / App.SKILL_MAX_LEVEL;

    // 成長タイプ補正
    switch (config.GrowthType) {
      case Growth.Fast: rate = Mathf.Pow(rate, GROWTH_FAST_FACTOR); break;
      case Growth.Slow: rate = Mathf.Pow(rate, GROWTH_SLOW_FACTOR); break;
      default: break;
    }

    return (int)Mathf.Lerp(0, config.MaxExp, rate);
  }

  /// <summary>
  /// Lvに応じたリキャストタイムを計算
  /// </summary>
  public static float CalcRecastTimeBy(ISkillEntityRO config, int lv)
  {
    return LerpParam(config.FirstRecastTime, config.LastRecastTime, lv);
  }

  /// <summary>
  /// Lvに応じたパワーを計算
  /// </summary>
  public static int CalcPowerBy(ISkillEntityRO config, int lv)
  {
    return (int)LerpParam(config.FirstPower, config.LastPower, lv);
  }

  /// <summary>
  /// Lvに応じた貫通数を計算
  /// </summary>
  public static int CalcPenetrableCount(ISkillEntityRO config, int lv)
  {
    return (int)LerpParam(config.FirstPenetrableCount, config.LastPenetrableCount, lv);
  }

  /// <summary>
  /// Lvに応じた速度補正値を計算
  /// </summary>
  public static float CalcSpeedCorrectionValue(ISkillEntityRO config, int lv)
  {
    return LerpParam(1f, config.SpeedGrowthRate, lv);
  }

  /// <summary>
  /// Lvからパラメータを計算する
  /// </summary>
  private static float LerpParam(float min, float max, int lv)
  {
    // 最大レベルに対する比率
    float rate = lv / (float)(App.SKILL_MAX_LEVEL);
    return Mathf.Lerp(min, max, rate);
  }
}
