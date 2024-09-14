using UnityEngine;

public class EnemyStatus
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// Enemy Master
  /// </summary>
  private IEnemyEntityRO master;

  /// <summary>
  /// Lv
  /// </summary>
  private int lv = 1;

  /// <summary>
  /// 体力、0になったら死亡する
  /// </summary>
  private readonly RangedFloat hp = new(0);

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
  

  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// EnemyID
  /// </summary>
  public EnemyId Id => master.Id;

  /// <summary>
  /// HP
  /// </summary>
  public RangedFloat Hp => hp;

  /// <summary>
  /// PowerにはLvがかかる
  /// </summary>
  private float Power => master.Power * lv;

  /// <summary>
  /// 速さ
  /// </summary>
  public float Speed => master.Speed;

  /// <summary>
  /// 質量
  /// </summary>
  public float Mass => master.Mass;

  /// <summary>
  /// 倒した時に得られるスキル
  /// </summary>
  public SkillId SkillId => master.SkillId;

  /// <summary>
  /// 経験値にはレベルがかかる
  /// </summary>
  public int Exp => master.Exp * lv;

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
    master = EnemyMaster.FindById(_id);

    SetLv(lv);
    hp.Init(master.HP);

    // 属性
    attrA.Value = master.AttackAttr;
    attrW.Value = master.WeakAttr;
    attrR.Value = master.ResistAttr;
    attrN.Value = master.NullfiedAttr;
  }

  /// <summary>
  /// Lvは1～MAX
  /// </summary>
  public void SetLv(int _lv)
  {
    lv = Mathf.Clamp(_lv, 1, App.ENEMY_MAX_LEVEL);
  }

  /// <summary>
  /// 攻撃情報を作る
  /// </summary>
  public AttackInfo MakeAttackInfo()
  {
    var status = new AttackInfo(){
      Power      = Power,
      Attributes = attrA.Value,
      Impact     = Mass * Speed,
    };
    
    return status;
  }

  /// <summary>
  /// ダメージを受ける
  /// </summary>
  public DamageInfo TakeDamage(AttackInfo info)
  {
    float        damage = info.Power;
    DamageDetail detail = DamageDetail.NormalDamage;


    // 無効属性かどうか
    if (attrN.HasEither(info.Attributes)) {
      damage = 0;
      detail = DamageDetail.NullfiedDamage;
    }

    // 耐性属性かどうか
    if (attrR.HasEither(info.Attributes)) {
      damage *= 0.5f;
      detail  = DamageDetail.ResistanceDamage;
    }

    // 弱点属性かどうか
    if (attrW.HasEither(info.Attributes)) {
      damage *= 3.0f;
      detail  = DamageDetail.WeaknessDamage;
    }

    hp.Now -= damage;

    return new DamageInfo(damage, detail);
  }

  /// <summary>
  /// {attributes}が弱点属性に含まれるならばtrue
  /// </summary>
  public bool IsWeakness(uint attributes)
  {
    return attrW.HasEither(attributes);
  }
}
