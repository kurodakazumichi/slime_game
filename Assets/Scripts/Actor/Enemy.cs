using System;
using UnityEngine;
using MyGame.Core.Props;

/// <summary>
/// 敵の基底クラス
/// </summary>
public abstract class Enemy<T> : MyMonoBehaviour, IEnemy
{
  //============================================================================
  // Const
  //============================================================================

  /// <summary>
  /// ノックバック減衰率
  /// </summary>
  private const float KNOCKBACK_ATTENUATION = 0.95f;

  /// <summary>
  /// ダメージ後の無敵時間
  /// </summary>
  private const float INVINCIBILITY_TIME_AFTER_DAMAGE = 0.2f;

  /// <summary>
  /// 攻撃後の休憩時間
  /// </summary>
  protected const float POST_ATTACK_REST_TIME = 0.4f;

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// 敵のメインビジュアルを表示しているSpriteRenderer
  /// </summary>
  private SpriteRenderer spriteRenderer;

  /// <summary>
  /// 敵のステータス
  /// </summary>
  protected EnemyStatus status = new();

  /// <summary>
  /// 速度
  /// </summary>
  protected Vector3 velocity;

  /// <summary>
  /// ノックバック速度
  /// </summary>
  protected Vector3 knockbackVelocity = Vector3.zero;

  /// <summary>
  /// 休憩タイマー、稼働中は動かない
  /// </summary>
  protected Timer restTimer = new();

  /// <summary>
  /// ノックバックタイマー
  /// </summary>
  protected Timer knockbackTimer = new();

  /// <summary>
  /// 無敵タイマー
  /// </summary>
  protected Timer invincibilityTimer = new();

  /// <summary>
  /// 丸影
  /// </summary>
  private Shadow shadow = null;

  /// <summary>
  /// State用タイマー
  /// </summary>
  protected float timer = 0f;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 識別子
  /// </summary>
  public EnemyId Id {
    get { return status.Id; }
  }

  /// <summary>
  /// 敵の持つスキルID
  /// </summary>
  public SkillId SkillId {
    get { return status.SkillId; }
  }

  /// <summary>
  /// 敵の持つ経験値
  /// </summary>
  public int Exp {
    get { return status.Exp; }
  }

  /// <summary>
  /// 速度
  /// </summary>
  public Vector3 Velocity {
    get { return velocity; }
  }

  /// <summary>
  /// 死亡時のコールバック
  /// </summary>
  public Action<IEnemy> OnDead { protected get; set; } = null;

  /// <summary>
  /// ステートマシン
  /// </summary>
  protected StateMachine<T> StateMachine { get; private set; } = new();

  /// <summary>
  /// 所属するWave
  /// </summary>
  protected EnemyWave OwnerWave { get; private set; } = null;

  /// <summary>
  /// 攻撃ステータス
  /// </summary>
  protected AttackInfo AttackInfo { get; private set; }

  /// <summary>
  /// 敵に設定されているコライダー
  /// </summary>
  protected SphereCollider Collider { get; private set; }

  /// <summary>
  /// 表示制御
  /// </summary>
  protected bool IsVisible 
  {
    get {
      return spriteRenderer.enabled;
    }

    set {
      spriteRenderer.enabled = value;
    }
  }

  /// <summary>
  /// 衝突制御
  /// </summary>
  protected bool IsCollidable 
  {
    get {
      return Collider.enabled;
    }
    set {
      Collider.enabled = value;
    }
  }

  /// <summary>
  /// 終了要求がきているならばtrue
  /// </summary>
  protected bool IsTerminationRequested 
  {
    get {
      if (OwnerWave == null) {
        return false;
      }

      return OwnerWave.IsTerminating;
    }
  }

  /// <summary>
  /// Playerと接触しているならばtrue
  /// </summary>
  protected bool IsIntersectedWithPlayer 
  {
    get 
    {
      var pm = PlayerManager.Instance;

      if (pm is null) {
        return false;
      }

      return CollisionUtil.IsCollideAxB(
        PlayerManager.Instance.Position,
        Position,
        Collider.radius
      );
    }
  }

  /// <summary>
  /// 透明度
  /// </summary>
  public float Alpha {
    set {
      var c = spriteRenderer.color;
      c.a = value;
      spriteRenderer.color = c;
    }
  }

  //----------------------------------------------------------------------------
  // 短縮系

  protected PlayerManager PM => PlayerManager.Instance;


  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// 初期化
  /// </summary>
  public void Init(EnemyId id, int lv) 
  {
    Logger.Log($"[Enemy] Called Init({id.ToString()})");
    status.Init(id, lv);
    AttackInfo = status.MakeAttackInfo();
  }

  /// <summary>
  /// このメソッドを呼び出すと敵が動きだすように実装すること
  /// </summary>
  public abstract void Run();

  /// <summary>
  /// 所属Waveをセットする
  /// </summary>
  public void SetOwnerWave(EnemyWave wave) { 
    OwnerWave = wave; 
  }
  
  /// <summary>
  /// ダメージを受ける
  /// </summary>
  public DamageInfo TakeDamage(AttackInfo info)
  {
    var result = CalcDamage(info);

    if (!result.IsHit) {
      return result;
    }

    // ダメージがある場合は無敵化
    if (result.HasDamage) {
      BeInvincible();
    }

    // 当たっていたらヒットテキストとノックバック
    PopOutHitText(result);
    SetupKnockbackVelocity(info);

    return result;
  }

  /// <summary>
  /// 敵を殺す
  /// </summary>
  public void Kill()
  {
    status.Hp.Empty();
  }

  /// <summary>
  /// {attributes}が弱点属性に含まれるならばtrue
  /// </summary>
  public bool IsWeakness(uint attributes)
  {
    return status.IsWeakness(attributes);
  }

  //----------------------------------------------------------------------------
  // Protected
  //----------------------------------------------------------------------------

  /// <summary>
  /// 影を落とす
  /// </summary>
  protected void CastShadow()
  {
    if (ShadowManager.Instance == null) {
      return;
    }

    ReleaseShadow();

    shadow = ShadowManager.Instance.Get();
    shadow.SetOwner(this, Collider.radius * 2f);
  }

  /// <summary>
  /// 影を解放する
  /// </summary>
  protected void ReleaseShadow()
  {
    if (ShadowManager.Instance == null) {
      return;
    }

    ShadowManager.Instance.Release(shadow);
    shadow = null;
  }

  /// <summary>
  /// 解放
  /// </summary>
  protected void Release()
  {
    if (OwnerWave != null) {
      OwnerWave.Release(this);
    }
    else {
      EnemyManager.Instance.Release(this);
    }

    ReleaseShadow();
  }

  /// <summary>
  /// 死亡時の処理、経験値を付与して解放
  /// </summary>
  protected void Die()
  {
    // 強制終了でなければ死亡時コールバックを実行
    if (!IsTerminationRequested) {
      OnDead?.Invoke(this);
    }
    
    Release();
  }

  /// <summary>
  /// 全てのタイマーを更新
  /// </summary>
  protected void UpdateTimer()
  {
    var dt = TimeSystem.Enemy.DeltaTime;

    knockbackTimer.Update(dt);
    invincibilityTimer.Update(dt);
    restTimer.Update(dt);
  }

  /// <summary>
  /// 全てのタイマーを停止
  /// </summary>
  protected void StopTimer()
  {
    knockbackTimer.Stop();
    invincibilityTimer.Stop();
    restTimer.Stop();
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    // コンポーネント収集
    Collider       = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    spriteRenderer.transform.rotation = Quaternion.Euler(App.CAMERA_ANGLE_X, 0, 0);

    SetupKnockbackTimer();
  }

  // Update is called once per frame
  void Update()
  {
    StateMachine.Update();
  }

  //----------------------------------------------------------------------------
  // Other
  //----------------------------------------------------------------------------

  private void SetupKnockbackVelocity(AttackInfo info)
  {
    // ノックバックの強さ
    var norm = info.Impact / status.Mass;

    // ノックバックの強さが1以下の場合、ノックバックは発生しない。
    if (norm <= 1f) {
      return;
    }

    // ノックバック速度を決定(PlayerとBulletの位置関係から決まる)
    var fromPlayer = (Position - PlayerManager.Instance.Position);
    var fromBullet = (Position - info.Position);

    knockbackVelocity = (fromPlayer.normalized * 0.6f) + (fromBullet.normalized * 0.4f);
    knockbackVelocity = knockbackVelocity * norm;

    // ノックバックタイマーを設定
    var time = MyMath.CalcDecayTime(norm, KNOCKBACK_ATTENUATION);
    knockbackTimer.Start(time);

    // ノックバック開始時に休憩終了
    restTimer.Stop();

    Logger.Log($"[Enemy.SetupKnockbackVelocity] norm {norm} time={time}");
  }

  /// <summary>
  /// HitTextを出す
  /// </summary>
  private void PopOutHitText(DamageInfo info)
  {
    var position = Position + Vector3.up;
    HitTextManager.Instance.Get().SetDisplay(position, (int)info.Damage);
  }

  /// <summary>
  /// ダメージ計算
  /// </summary>
  private DamageInfo CalcDamage(AttackInfo info)
  {
    if (invincibilityTimer.IsRunning) {
      return new DamageInfo(0f, DamageDetail.NullfiedByInvincibility);
    }

    return status.TakeDamage(info);
  }

  /// <summary>
  /// ノックバックタイマーのセットアップ
  /// </summary>
  private void SetupKnockbackTimer()
  {
    // 更新時はノックバック速度を減衰
    knockbackTimer.OnUpdate = (rate) => {
      knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, rate);
    };

    // 終了時はノックバック速度を0に
    knockbackTimer.OnStop = () => {
      knockbackVelocity = Vector3.zero;
    };
  }

  /// <summary>
  /// 無敵になる
  /// </summary>
  private void BeInvincible()
  {
    spriteRenderer.color = Color.red;
    invincibilityTimer.Start(INVINCIBILITY_TIME_AFTER_DAMAGE);
    invincibilityTimer.OnStop = () => spriteRenderer.color = Color.white;
  }
}