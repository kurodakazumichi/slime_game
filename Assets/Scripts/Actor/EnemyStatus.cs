using UnityEngine;

public class EnemyStatus
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// 識別子
  /// </summary>
  private EnemyId id;

  /// <summary>
  /// Lv
  /// </summary>
  private int lv = 1;

  /// <summary>
  /// 体力、0になったら死亡する
  /// </summary>
  private RangedFloat hp { get; set; } = new(0);

  /// <summary>
  /// 攻撃力、プレイヤーへの攻撃に使用する
  /// </summary>
  private float power = 0;

  /// <summary>
  /// 攻撃属性
  /// </summary>
  private Flag32 attrA = new();

  /// <summary>
  /// 弱点属性、攻撃を受ける時に参照
  /// </summary>
  private Flag32 attrW = new();

  /// <summary>
  /// 耐性属性、攻撃を受ける時に参照
  /// </summary>
  private Flag32 attrR = new();

  /// <summary>
  /// 無効属性、攻撃を受ける時に参照
  /// </summary>
  private Flag32 attrN = new();

  /// <summary>
  /// 倒されたときに付与するスキルのID
  /// </summary>
  private SkillId skillId = SkillId.Undefined;

  /// <summary>
  /// 倒されたときに付与される経験値
  /// </summary>
  private int exp = 0;

  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// EnemyID
  /// </summary>
  public EnemyId Id => id;

  /// <summary>
  /// PowerにはLvがかかる
  /// </summary>
  private float Power => power * lv;

  /// <summary>
  /// HP
  /// </summary>
  public RangedFloat Hp => hp;

  /// <summary>
  /// 倒した時に得られるスキル
  /// </summary>
  public SkillId SkillId => skillId;

  /// <summary>
  /// 経験値にはレベルがかかる
  /// </summary>
  public int Exp => exp * lv;

  /// <summary>
  /// 死亡フラグ
  /// </summary>
  public bool IsDead => Hp.IsEmpty;

  //============================================================================
  // Methods
  //============================================================================
  /// <summary>
  /// ステータスの初期化
  /// </summary>
  public void Init(EnemyId _id, int lv)
  {
    id = _id;
    SetLv(lv);

    var master = EnemyMaster.FindById(id);

    hp.Init(master.HP);
    attrA.Value = master.AttackAttr;
    attrW.Value = master.WeakAttr;
    attrR.Value = master.ResistAttr;
    attrN.Value = master.NullfiedAttr;
    power       = master.Power;
    skillId     = master.SkillId;
    exp         = master.Exp;
  }

  /// <summary>
  /// Lvは1～MAX
  /// </summary>
  public void SetLv(int _lv)
  {
    lv = Mathf.Clamp(_lv, 1, App.ENEMY_MAX_LEVEL);
  }

  /// <summary>
  /// 攻撃用ステータスを作る
  /// </summary>
  public AttackStatus MakeAttackStatus()
  {
    var status = new AttackStatus();
    status.Init(Power, attrA.Value);
    return status;
  }

  /// <summary>
  /// ダメージを受ける
  /// </summary>
  public float TakeDamage(AttackStatus p)
  {
    float damage = p.Power;

    // 無効属性かどうか
    if (attrN.HasEither(p.Attributes)) {
      damage = 0;
    }

    // 耐性属性かどうか
    if (attrR.HasEither(p.Attributes)) {
      damage *= 0.5f;
    }

    // 弱点属性かどうか
    if (attrW.HasEither(p.Attributes)) {
      damage *= 3.0f;
    }

    hp.Now -= damage;
    return damage;
  }
}
