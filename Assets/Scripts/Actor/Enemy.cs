using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// 敵の基底クラス
/// </summary>
public abstract class Enemy<T> : MyMonoBehaviour, IEnemy
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// ステートマシン
  /// </summary>
  protected StateMachine<T> state { get; private set; } = new();

  /// <summary>
  /// 敵に設定されているコライダー
  /// </summary>
  new protected SphereCollider collider { get; private set; }

  /// <summary>
  /// 敵のメインビジュアルを表示しているSpriteRenderer
  /// </summary>
  private SpriteRenderer spriteRenderer;

  /// <summary>
  /// 敵のステータス
  /// </summary>
  protected EnemyStatus status = new();

  /// <summary>
  /// 所属するWave
  /// </summary>
  protected EnemyWave ownerWave { get; private set; } = null;

  /// <summary>
  /// 攻撃ステータス
  /// </summary>
  protected AttackInfo attackInfo { get; private set; } = null;

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
  /// 表示制御
  /// </summary>
  protected bool isVisible 
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
  protected bool isCollidable 
  {
    get {
      return collider.enabled;
    }
    set {
      collider.enabled = value;
    }
  }

  /// <summary>
  /// 終了要求がきているならばtrue
  /// </summary>
  protected bool isTerminationRequested 
  {
    get {
      if (ownerWave == null) {
        return false;
      }

      return ownerWave.IsTerminating;
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
    attackInfo = status.MakeAttackInfo();
  }

  /// <summary>
  /// このメソッドを呼び出すと敵が動きだすように実装すること
  /// </summary>
  public abstract void Run();

  /// <summary>
  /// 所属Waveをセットする
  /// </summary>
  public void SetOwnerWave(EnemyWave wave) { 
    ownerWave = wave; 
  }

  /// <summary>
  /// ダメージを受ける
  /// </summary>
  public void TakeDamage(AttackInfo info)
  {
    var damage = status.TakeDamage(info);

    var position = CachedTransform.position + Vector3.up;
    HitTextManager.Instance.Get().SetDisplay(position, (int)damage);
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
    if (ownerWave != null) {
      ownerWave.Release(this);
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
    SkillManager.Instance.StockExp(status.SkillId, status.Exp);
    Release();
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    // コンポーネント収集
    collider       = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    spriteRenderer.transform.rotation = Quaternion.Euler(45f, 0, 0);
  }

  // Update is called once per frame
  void Update()
  {
    state.Update();
  }
}