

/// <summary>
/// Skill Entity
/// </summary>
public class SkillEntity : ISkillEntity
{
  /// <summary>
  /// Skill ID
  /// </summary>
  public SkillId Id { get; set; }

  /// <summary>
  /// LvMaxに必要な経験値
  /// </summary>
  public int MaxExp { get; set; }

  /// <summary>
  /// Lv Min時のリキャストタイム
  /// </summary>
  public float FirstRecastTime { get; set; }

  /// <summary>
  /// Lv Max時のリキャストタイム
  /// </summary>
  public float LastRecastTime { get; set; }

  /// <summary>
  /// Lv Min時のパワー
  /// </summary>
  public int FirstPower { get; set; }

  /// <summary>
  /// Lv Max時のパワー
  /// </summary>
  public int LastPower { get; set; }

  /// <summary>
  /// Lv Min時 貫通数
  /// </summary>
  public int FirstPenetrableCount { get; set; }

  /// <summary>
  /// Lv Max時 貫通数
  /// </summary>
  public int LastPenetrableCount { get; set; }

  /// <summary>
  /// 速度成長率
  /// </summary>
  public float SpeedGrowthRate { get; set; }

  /// <summary>
  /// 属性
  /// </summary>
  public uint Attr { get; set; }

  /// <summary>
  /// 名称
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Prefabのパス
  /// </summary>
  public string Prefab { get; set; }

  /// <summary>
  /// スキルの成長タイプ
  /// </summary>
  public Growth GrowthType { get; set; }

  /// <summary>
  /// ヒット時の衝撃[N]
  /// </summary>
  public float Impact { get; set; }

  /// <summary>
  /// アイコン番号
  /// </summary>
  public int IconNo { get; set; }

  /// <summary>
  /// 狙う相手のタイプ
  /// </summary>
  public SkillAimingType Aiming { get; set; }
}

