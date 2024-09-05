using UnityEngine;

public interface ISkill
{
  SkillId Id { get; }
  int Lv { get; }
  float RecastTime { get; }
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
  public SkillId Id {
    get { return entity.Id; }
  }

  /// <summary>
  /// 属性
  /// </summary>
  public uint Attributes {
    get { return entity.Attr; }
  }

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
  public virtual void Fire()
  {
    var bullet = BulletManager.Instance.Get(Id);
    bullet.Fire(this);
  }

  /// <summary>
  /// 経験値をセット、経験値に応じてスキルパラメータを再計算。
  /// </summary>
  public void SetExp(int exp)
  {
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
    return Mathf.Lerp(min, max, lv / (float)(App.SKILL_MAX_LEVEL));
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
    lv = Mathf.Clamp(lv, 0, App.SKILL_MAX_LEVEL);
    return (int)Mathf.Lerp(0, entity.MaxExp, (float)(lv) / App.SKILL_MAX_LEVEL);
  }
}
