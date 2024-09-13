using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// 敵の基底クラス
/// </summary>
public abstract class Enemy<T> : MyMonoBehaviour, IEnemy
{
  /// <summary>
  /// ノックバック減衰率
  /// </summary>
  private const float KNOCKBACK_ATTENUATION = 0.95f;

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
  protected Vector3 KnockbackVelocity = Vector3.zero;

  /// <summary>
  /// ノックバックタイマー
  /// </summary>
  protected Timer knockbackTimer = new();

  //============================================================================
  // Properities
  //============================================================================

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
  protected AttackInfo AttackInfo { get; private set; } = null;

  /// <summary>
  /// 識別子
  /// </summary>
  public EnemyId Id {
    get { return status.Id; } 
  }

  /// <summary>
  /// 速度
  /// </summary>
  public Vector3 Velocity 
    { get { return velocity; } 
  }

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
    var result = status.TakeDamage(info);

    SetupKnockbackVelocity(info);

    PopOutHitText((int)result.Damage);

    return result;
  }

  /// <summary>
  /// 敵を殺す
  /// </summary>
  public void Kill()
  {
    status.Hp.Empty();
  }

  //----------------------------------------------------------------------------
  // Protected
  //----------------------------------------------------------------------------

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
  }

  /// <summary>
  /// 死亡時の処理、経験値を付与して解放
  /// </summary>
  protected void Die()
  {
    SkillManager.Instance.AddExp(status.SkillId, status.Exp);
    Release();
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    // コンポーネント収集
    Collider = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    spriteRenderer.transform.rotation = Quaternion.Euler(45f, 0, 0);
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

    // ノックバック速度とノックバックタイマーを設定
    KnockbackVelocity
      = (Position - PlayerManager.Instance.Position)
        .normalized * norm;

    var time = MyMath.CalcDecayTime(norm, KNOCKBACK_ATTENUATION);
    knockbackTimer.Start(time);

    Logger.Log($"[Enemy.SetupKnockbackVelocity] norm {norm} time={time}");
  }

  /// <summary>
  /// HitTextを出す
  /// </summary>
  private void PopOutHitText(int damage)
  {
    var position = Position + Vector3.up;
    HitTextManager.Instance.Get().SetDisplay(position, damage);
  }
}