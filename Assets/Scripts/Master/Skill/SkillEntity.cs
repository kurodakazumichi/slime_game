using MyGame;
using UnityEngine;

namespace MyGame.Master 
{
  public class SkillEntity : ScriptableObject, ISkillEntity, IConvertibleCsvText
  {
    //=========================================================================
    // Inspector
    //=========================================================================
    public string _id;
    public int _no;
    public string _name;
    public int _maxExp;
    public float _recastF;
    public float _recastL;
    public int _powerF;
    public int _powerL;
    public int _penetrableF;
    public int _penetrableL;
    public float _speedGrowthRate;
    public string _attributes;
    public string _growthType;
    public float _impact;
    public int _iconNo;
    public string _aimingType;

    //=========================================================================
    // Variables that need to be converted.
    // Inspectorで設定された文字列やデータの中で変換する必要があるパラメータを定義
    // 以下に定義されたデータはInitのタイミングで使える形式に変換される。
    //=========================================================================
    private SkillId id;
    private Growth growthType;
    private SkillAimingType aimingType;
    private uint attributes;
    private string prefab;

    //=========================================================================
    // Property
    //=========================================================================
    public SkillId Id => id;

    public int MaxExp => _maxExp;

    public float FirstRecastTime => _recastF;

    public float LastRecastTime => _recastL;

    public int FirstPower => _powerF;

    public int LastPower => _powerL;

    public int FirstPenetrableCount => _penetrableF;

    public int LastPenetrableCount => _penetrableL;

    public float SpeedGrowthRate => _speedGrowthRate;

    public uint Attr => attributes;

    public string Name => _name;

    public string Prefab => prefab;

    public Growth GrowthType => growthType;

    public float Impact => _impact;

    public int IconNo => _iconNo;

    public SkillAimingType Aiming => aimingType;

    //=========================================================================
    // Method
    //=========================================================================
    public void Init()
    {
      if (!MyEnum.TryParse<SkillId>(_id, out id)) {
        Logger.Error($"SkillId {_id} parse failed.");
      }

      if (!MyEnum.TryParse<Growth>(_growthType, out growthType)) {
        Logger.Error($"Growth {_growthType} parse failed.");
      }

      if (!MyEnum.TryParse<SkillAimingType>(_aimingType, out aimingType)) {
        Logger.Error($"SkillAimingType {_aimingType} parse failed.");
      }

      attributes = AttributeUtil.GetAttributesFromString(_attributes);
      prefab = $"Bullet/{_id}/Object.prefab";
    }

    //=========================================================================
    // ToString
    //=========================================================================
    public static string CsvHeaderString()
    {
      return "Id,No,Name,MaxExp,RecastF,RecastL,PowerF,PowerL,PenetrableF,PenetrableL,SpeedGrowthRate,Attributes,GrowthType,Impact,IconNo,AimingType";
    }

    public string ToCsvText()
    {
      string[] datas = {
        _id,
        _no.ToString(),
        _name,
        _maxExp.ToString(),
        _recastF.ToString(),
        _recastL.ToString(),
        _powerF.ToString(),
        _powerL.ToString(),
        _penetrableF.ToString(),
        _penetrableL.ToString(),
        _speedGrowthRate.ToString(),
        _attributes.ToString(),
        _growthType,
        _impact.ToString(),
        _iconNo.ToString(),
        _aimingType,
      };

      return string.Join(",", datas);
    }
  }
}


namespace Old
{
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
}
