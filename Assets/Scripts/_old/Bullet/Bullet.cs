﻿using UnityEngine;

/// <summary>
/// 弾丸の基底クラス
/// </summary>
public abstract class Bullet<T> : MyMonoBehaviour, IBullet
{
  //============================================================================
  // Variables for Inspector
  //============================================================================
  [SerializeField, Tooltip("最低速度")]
  private float MinSpeed = 1.0f;

  [SerializeField, Tooltip("最大速度")]
  private float MaxSpeed = 1.0f;

  [SerializeField, Tooltip("速度変化時間")]
  private float SpeedChangeTime = 1.0f;

  [SerializeField, Tooltip("速度変化カーブ")]
  public AnimationCurve SpeedChangeCurve = new AnimationCurve();

  [SerializeField, Tooltip("追尾性能"), Range(0f, 1f)]
  protected float HomingPerformance = 0f;

  [SerializeField, Tooltip("追尾時間")]
  protected float HomingTime = 0f;

  [SerializeField, Tooltip("寿命、0を設定すると寿命なし")]
  protected float LifeTime = 0f;

  [SerializeField, Tooltip("方向と回転を同期する")]
  protected bool SyncDirectionAndRotation = false;

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// 終了フラグ、このフラグがtrueになったら処理を中断し速やかにIdle状態に遷移する
  /// </summary>
  protected bool isTerminating = false;

  /// <summary>
  /// 汎用タイマー
  /// </summary>
  protected float timer = 0f;

  /// <summary>
  /// 方向
  /// </summary>
  protected Vector3 direction = Vector3.zero;

  /// <summary>
  /// 速度
  /// </summary>
  protected Vector3 velocity = Vector3.zero;

  /// <summary>
  /// 速度補正値
  /// </summary>
  private float speedCorrection = 1f;

  /// <summary>
  /// 貫通可能数
  /// </summary>
  private int penetrableCount = 0;

  /// <summary>
  /// 丸影
  /// </summary>
  protected Shadow shadow = null;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// スキルID
  /// </summary>
  public SkillId Id { get; private set; }

  /// <summary>
  /// ステートマシン
  /// </summary>
  protected StateMachine<T> StateMachine { get; private set; }

  /// <summary>
  /// 派生先で弾丸がIdle状態ならばtrueを返す処理を定義すること
  /// </summary>
  public abstract bool IsIdle { get; }

  /// <summary>
  /// 表示、非表示を制御するフラグ
  /// </summary>
  protected abstract bool IsVisible { get; set; }

  /// <summary>
  /// 衝突するか、しないかを制御するフラグ
  /// </summary>
  protected abstract bool IsCollidable { get; set; }

  /// <summary>
  /// 貫通可能か？
  /// </summary>
  private bool IsPenetrable {
    get { return 0 < penetrableCount; }
  }

  /// <summary>
  /// BulletがPlayerのものか、Enemyものかを制御するプロパティ
  /// </summary>
  protected BulletOwner Owner {
    get {
      return (layer == Layer.EnemyBullet) ? BulletOwner.Enemy : BulletOwner.Player;
    }
    set {
      layer = (value == BulletOwner.Enemy) ? Layer.EnemyBullet : Layer.PlayerBullet;
    }
  }

  /// <summary>
  /// ターゲット、いない場合はnull
  /// </summary>
  protected IActor Target { get; private set; } = null;

  /// <summary>
  /// 攻撃情報
  /// </summary>
  protected AttackInfo AttackInfo;

  //============================================================================
  // Methods
  //============================================================================

  public virtual void Fire(BulletFireInfo info)
  {
    SetStatusBy(info.Skill);
    Owner                    = info.Owner;
    Target                   = info.Target;
    CachedTransform.position = info.Position;
    direction                = info.Direction;
  }

  public void Terminate()
  {
    if (IsIdle) {
      return;
    }
    isTerminating = true;
  }

  /// <summary>
  /// Actorに対して攻撃を行う
  /// </summary>
  public void Attack(IActor actor)
  {
    if (isTerminating) {
      return;
    }

    AttackInfo.Position = CachedTransform.position;
    var result = actor.TakeDamage(AttackInfo);

    // 攻撃があたった場合
    if (result.IsHit) {
      isTerminating = !IsPenetrable;
      penetrableCount--;
    }
  }

  /// <summary>
  /// 衝突時の処理を定義すること
  /// </summary>
  public abstract void Intersect();

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    StateMachine = new StateMachine<T>();
  }

  void Update()
  {
    StateMachine.Update();
  }

  //----------------------------------------------------------------------------
  // 物理系
  //----------------------------------------------------------------------------

  /// <summary>
  /// 速度を計算する
  /// </summary>
  protected float CalcSpeed(float timer)
  {
    // 最低速度、最高速度に補正をかける
    var MinSpeed = this.MinSpeed * speedCorrection;
    var MaxSpeed = this.MaxSpeed * speedCorrection;

    if (SpeedChangeTime <= 0) {
      return MinSpeed;
    }

    if (MinSpeed == MaxSpeed) {
      return MinSpeed;
    }

    // 経過時間と速度変化カーブから速度を求める
    var rate = Mathf.Min(1.0f, timer / SpeedChangeTime);
    return Mathf.Lerp(MinSpeed, MaxSpeed, SpeedChangeCurve.Evaluate(rate));
  }

  /// <summary>
  /// 方向を計算する
  /// </summary>
  protected Vector3 CalcDirection(float timer)
  {
    // ターゲットが存在しなければ方向維持
    if (Target is null || !Target.gameObject.activeSelf) {
      return direction;
    }

    // ホーミング性能がなければ方向維持
    if (HomingPerformance <= 0) {
      return direction;
    }

    // ホーミング時間が過ぎたら方向維持
    if (HomingTime < timer) {
      return direction;
    }

    // ターゲットに向かうベクトルを求める
    var toTarget = (Target.Position - Position).normalized;

    // 最大パフォーマンスだったら即座に速度を向ける
    if (1f <= HomingPerformance) {
      return toTarget;
    }

    // なんかよくわからん式、↓を参考に色々やったらなんかいい感じだからいいかなって諦めた
    // https://logmi.jp/tech/articles/326878
    var beta = 1f - Mathf.Pow(1-(HomingPerformance*0.1f), 60f*TimeSystem.Bullet.DeltaTime);
    return Vector3.Slerp(direction, toTarget, beta);
  }

  //----------------------------------------------------------------------------
  // for Me
  //----------------------------------------------------------------------------

  /// <summary>
  /// スキルに設定されているパラメータからステータスを設定する
  /// </summary>
  protected void SetStatusBy(ISkill skill)
  {
    Id              = skill.Id;
    penetrableCount = skill.PenetrableCount;
    speedCorrection = skill.SpeedCorrectionValue;

    AttackInfo = new AttackInfo() {
      Power      = skill.Power,
      Attributes = skill.Attributes,
      Impact     = skill.Impact,
    };
  }

  /// <summary>
  /// 寿命を越えました？
  /// </summary>
  protected bool IsLifeOver(float timer)
  {
    if (LifeTime <= 0) {
      return false;
    }

    return LifeTime < timer;
  }
}
