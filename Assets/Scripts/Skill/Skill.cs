using UnityEngine;

public interface ISkill
{
  SkillId Id { get; }
  int Lv { get; }
  float RecastTime { get; }
  string Name { get; }
  int Power { get; }
  int PenetrableCount { get; }
  float SpeedCorrectionValue { get; }
  float Impact { get; }
  uint Attributes { get; }
  void Fire();
  void SetExp(int exp);
  void SetLv(int lv);
  SkillAimingType Aiming { get; }
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
  /// Skillの設定情報
  /// </summary>
  protected ISkillEntityRO config;

  //============================================================================
  // Properties
  //============================================================================

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

  /// <summary>
  /// 貫通可能数
  /// </summary>
  public int PenetrableCount { get; private set; } = 0;

  /// <summary>
  /// 速度補正値
  /// </summary>
  public float SpeedCorrectionValue { get; private set; } = 0f;

  /// <summary>
  /// スキルID
  /// </summary>
  public SkillId Id => config.Id;

  /// <summary>
  /// 名称
  /// </summary>
  public string Name => config.Name;

  /// <summary>
  /// 属性
  /// </summary>
  public uint Attributes => config.Attr;

  /// <summary>
  /// 衝撃力[N]
  /// </summary>
  public float Impact => config.Impact;

  /// <summary>
  /// 狙い
  /// </summary>
  public SkillAimingType Aiming => config.Aiming;

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
    this.config = entity;
    SetExp(exp);
  }

  /// <summary>
  /// スキル発動
  /// </summary>
  public void Fire()
  {
    var bullet   = BulletManager.Instance.Get(Id);
    var position = PlayerManager.Instance.Position;

    IActor target = null;
    Vector3 direction = MyVector3.Random(Vector3.forward, Vector3.up);

    // スキルに設定されているAimingType毎にtargetを設定
    switch (Aiming) {
      case SkillAimingType.Nearest: 
        target = EnemyManager.Instance.FindNearest(position); break;
      case SkillAimingType.Weakest: 
        target = EnemyManager.Instance.FindWeakness(position, config.Attr); break;
      case SkillAimingType.Player: 
        target = PlayerManager.Instance.Player; break;
      default:
        target = EnemyManager.Instance.FindRandom(); break;
    }

    // 発射!!
    bullet.Fire(new BulletFireInfo() {
      Position  = position,
      Direction = direction,
      Skill     = this,
      Target    = target,
      Owner     = BulletOwner.Player,
    });
  }

  /// <summary>
  /// 経験値をセット、経験値に応じてスキルパラメータを再計算。
  /// </summary>
  public void SetExp(int exp)
  {
    exp                  = Mathf.Max(0, exp);
    Lv                   = CalcLevelBy(exp);
    RecastTime           = CalcRecastTimeBy(Lv);
    Power                = CalcPowerBy(Lv);
    PenetrableCount      = CalcPenetrableCount(Lv);
    SpeedCorrectionValue = CalcSpeedCorrectionValue(Lv);
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
    return LerpParam(config.FirstRecastTime, config.LastRecastTime, lv);
  }

  /// <summary>
  /// Lvに応じたパワーを計算
  /// </summary>
  private int CalcPowerBy(int lv)
  {
    return (int)LerpParam(config.FirstPower, config.LastPower, lv);
  }

  /// <summary>
  /// Lvに応じた貫通数を計算
  /// </summary>
  private int CalcPenetrableCount(int lv)
  {
    return (int)LerpParam(config.FirstPenetrableCount, config.LastPenetrableCount, lv);
  }

  /// <summary>
  /// Lvに応じた速度補正値を計算
  /// </summary>
  private float CalcSpeedCorrectionValue(int lv)
  {
    return LerpParam(1f, config.SpeedGrowthRate, lv);
  }

  /// <summary>
  /// Lvに必要な経験値を取得
  /// </summary>
  private int GetNeedExp(int lv)
  {
    return SkillManager.GetNeedExp(config, lv);
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
      var entity = skill.config;
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
        GUILayout.Toggle(attributes.Has((int)Attribute.Lef), "草");
        GUILayout.Toggle(attributes.Has((int)Attribute.Hol), "聖");
        GUILayout.Toggle(attributes.Has((int)Attribute.Dar), "闇");
      }

      if (GUILayout.Button("Fire")) {
        skill.Fire();
      }
    }
  }

  /// <summary>
  /// デバッグ用のマニュアル発射
  /// </summary>
  public void ManualFire(Vector3 position, IActor target, BulletOwner owner = BulletOwner.Player)
  {
    var bullet = BulletManager.Instance.Get(Id);

    bullet.Fire(new BulletFireInfo() {
      Position  = position,
      Direction = MyVector3.Random(Vector3.forward, Vector3.up),
      Skill     = this,
      Target    = target,
      Owner     = owner,
    });
  }

#endif
}
