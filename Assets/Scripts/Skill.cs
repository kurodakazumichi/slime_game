using UnityEngine;

public interface ISkill
{
  SkillId Id { get; }
  int Lv { get; }
  float RecastTime { get; }
  string Name { get; }
  int Power { get; }
  uint Attributes { get; }
  void Fire();
  void SetExp(int exp);
  void SetLv(int lv);
}

/// <summary>
/// スキル
/// </summary>
public class Skill : ISkill
{

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// Skillの基本情報
  /// </summary>
  protected ISkillEntityRO entity;

  /// <summary>
  /// スキルLv(経験値をセットしたタイミングで設定される)
  /// </summary>
  public int Lv { get; private set; }

  /// <summary>
  /// スキルの強さ(経験値をセットしたタイミングで設定される)
  /// </summary>
  public int Power { get; private set; } = 0;

  /// <summary>
  /// リキャストタイム(経験値をセットしたタイミングで設定される)
  /// </summary>
  public float RecastTime { get; private set; } = 0f;


  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// スキルID
  /// </summary>
  public SkillId Id => entity.Id;

  /// <summary>
  /// 名称
  /// </summary>
  public string Name => entity.Name;

  /// <summary>
  /// 属性
  /// </summary>
  public uint Attributes => entity.Attr;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// 初期化
  /// </summary>
  public void Init(ISkillEntityRO entity, int exp)
  {
    this.entity = entity;
    SetExp(exp);
  }

  /// <summary>
  /// スキル発動
  /// </summary>
  public void Fire()
  {
    var bullet = BulletManager.Instance.Get(Id);

    var pm    = PlayerManager.Instance;
    var enemy = EnemyManager.Instance.FindNearestEnemy(pm.Position);

    bullet.Fire(new BulletFireInfo() {
      Position = pm.Position,
      Skill    = this,
      Target   = enemy,
      Owner    = BulletOwner.Player
    });
  }

  /// <summary>
  /// 経験値をセット、経験値に応じてスキルパラメータを再計算。
  /// </summary>
  public void SetExp(int exp)
  {
    exp        = Mathf.Max(0, exp);
    Lv         = CalcLevelBy(exp);
    RecastTime = CalcRecastTimeBy(Lv);
    Power      = CalcPowerBy(Lv);
  }

  /// <summary>
  /// Lvを設定、Lvに応じてパラメータを再計算
  /// </summary>
  /// <param name="lv"></param>
  public void SetLv(int lv)
  {
    lv = Mathf.Clamp(lv, 0, App.SKILL_MAX_LEVEL);
    SetExp(GetNeedExp(lv));
  }

  /// <summary>
  /// 経験値からレベルを計算
  /// </summary>
  private int CalcLevelBy(int exp)
  {
    for (int i = App.SKILL_MAX_LEVEL; 0 <= i; --i) {

      if (GetNeedExp(i) <= exp) {
        return i;
      }
    }

    return 0;
  }

  /// <summary>
  /// Lvからパラメータを計算する
  /// </summary>
  private float LerpParam(float min, float max, int lv)
  {
    // 最大レベルに対する比率
    float rate = lv / (float)(App.SKILL_MAX_LEVEL);
    return Mathf.Lerp(min, max, rate);
  }

  /// <summary>
  /// Lvに応じたリキャストタイムを計算
  /// </summary>
  private float CalcRecastTimeBy(int lv)
  {
    return LerpParam(entity.FirstRecastTime, entity.LastRecastTime, lv);
  }

  /// <summary>
  /// Lvに応じたパワーを計算
  /// </summary>
  private int CalcPowerBy(int lv)
  {
    return (int)LerpParam(entity.FirstPower, entity.LastPower, lv);
  }

  /// <summary>
  /// Lvに必要な経験値を取得
  /// </summary>
  private int GetNeedExp(int lv)
  {
    // 成長タイプ別係数
    const float GROWTH_FAST_FACTOR = 2.0f;
    const float GROWTH_SLOW_FACTOR = 0.5f;

    lv = Mathf.Clamp(lv, 0, App.SKILL_MAX_LEVEL);

    var rate = (float)(lv) / App.SKILL_MAX_LEVEL;

    // 成長タイプ補正
    switch (entity.GrowthType) {
      case Growth.Fast: rate = Mathf.Pow(rate, GROWTH_FAST_FACTOR); break;
      case Growth.Slow: rate = Mathf.Pow(rate, GROWTH_SLOW_FACTOR); break;
      default: break;
    }

    return (int)Mathf.Lerp(0, entity.MaxExp, rate);
  }

  /// <summary>
  /// 次のレベルの経験値
  /// </summary>
  private int GetNextExp()
  {
    return GetNeedExp(Lv + 1);
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public void OnDebug()
  {
    SkillDebugger.OnGUI(this);
  }

  public static class SkillDebugger
  {
    private static Flag32 attributes = new Flag32();

    private static void DrawProperty(string name, string value)
    {
      using (new GUILayout.HorizontalScope()) 
      {
        GUILayout.Label(name, GUILayout.Width(100));
        GUILayout.Label(value);
      }
    }

    public static void OnGUI(Skill skill)
    {
      var entity = skill.entity;
      attributes.Value = skill.Attributes;

      DrawProperty("Id"        , skill.Id.ToString());
      DrawProperty("Name"      , skill.Name);
      DrawProperty("Lv"        , $"{skill.Lv}");
      DrawProperty("Power"     , $"{skill.Power} ({entity.FirstPower} - {entity.LastPower})");
      DrawProperty("Recast"    , $"{skill.RecastTime} ({entity.FirstRecastTime} - {entity.LastRecastTime})");
      DrawProperty("Max Exp"   , $"{entity.MaxExp}");
      DrawProperty("Next Exp"  , $"{skill.GetNextExp()}");
      DrawProperty("GrowthType", $"{entity.GrowthType.ToString()}");
      DrawProperty("Prefab"    , $"{entity.Prefab}");

      using (new GUILayout.HorizontalScope()) 
      {
        GUILayout.Toggle(attributes.Has((int)Attribute.Non), "無");
        GUILayout.Toggle(attributes.Has((int)Attribute.Fir), "火");
        GUILayout.Toggle(attributes.Has((int)Attribute.Wat), "水");
        GUILayout.Toggle(attributes.Has((int)Attribute.Thu), "雷");
        GUILayout.Toggle(attributes.Has((int)Attribute.Ice), "氷");
        GUILayout.Toggle(attributes.Has((int)Attribute.Tre), "木");
        GUILayout.Toggle(attributes.Has((int)Attribute.Hol), "聖");
        GUILayout.Toggle(attributes.Has((int)Attribute.Dar), "闇");
      }

      if (GUILayout.Button("Fire")) {
        skill.Fire();
      }
    }
  }

#endif
}
