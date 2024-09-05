using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// 敵のインターフェース
/// </summary>
public interface IEnemy
{
  /// <summary>
  /// 識別子の取得
  /// </summary>
  EnemyId Id { get; }

  /// <summary>
  /// 初期化
  /// </summary>
  void Init(EnemyId id);

  /// <summary>
  /// ダメージを受ける
  /// </summary>
  void TakeDamage(AttackStatus p);

  /// <summary>
  /// 所属Waveを設定する
  /// </summary>
  void SetOwnerWave(EnemyWave wave);

  /// <summary>
  /// 活動を開始する
  /// </summary>
  void Run();

  /// <summary>
  /// 敵を殺す
  /// </summary>
  void Kill();

  //----------------------------------------------------------------------------
  // MonoBehaviour系

  GameObject gameObject { get; }
  Transform CachedTransform { get; }
}

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
  protected StateMachine<T> state { get; private set; }

  /// <summary>
  /// 敵に設定されているコライダー
  /// </summary>
  new protected SphereCollider collider { get; private set; }

  /// <summary>
  /// 敵のメインビジュアルを表示しているSpriteRenderer
  /// </summary>
  private SpriteRenderer spriteRenderer;

  /// <summary>
  /// 識別子
  /// </summary>
  private EnemyId id;
  
  /// <summary>
  /// 体力、0になったら死亡する
  /// </summary>
  protected RangedFloat hp { get; private set; }

  /// <summary>
  /// 攻撃力、プレイヤーへの攻撃に使用する
  /// </summary>
  private float power = 0;

  /// <summary>
  /// 攻撃属性
  /// </summary>
  private Flag32 attrA;

  /// <summary>
  /// 弱点属性、攻撃を受ける時に参照
  /// </summary>
  private Flag32 attrW;

  /// <summary>
  /// 無効属性、攻撃を受ける時に参照
  /// </summary>
  private Flag32 attrN;

  /// <summary>
  /// 倒されたときに付与するスキルのID
  /// </summary>
  private SkillId skillId;

  /// <summary>
  /// 倒されたときに付与される経験値
  /// </summary>
  private int exp;

  /// <summary>
  /// 所属するWave
  /// </summary>
  protected EnemyWave ownerWave { get; private set; } = null;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 識別子
  /// </summary>
  public EnemyId Id { get { return id; } }

  /// <summary>
  /// 見た目の位置
  /// </summary>
  public Vector3 VisualPosition {
    get { return CachedTransform.position + collider.center; }
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
  public void Init(EnemyId id) 
  {
    Logger.Log($"[Enemy] Called Init({id.ToString()})");
    this.id = id;

    var master = EnemyMaster.FindById(id);
    hp.Init(master.HP);
    attrA.Value = master.AttackAttr;
    attrW.Value = master.WeakAttr;
    attrN.Value = master.NullfiedAttr;
    power       = master.Power;
    skillId     = master.SkillId;
    exp         = master.Exp;
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
  public void TakeDamage(AttackStatus p)
  {
    HitTextManager.Instance.Get().SetDisplay(VisualPosition, (int)p.Power);

    hp.Now -= p.Power;
  }

  /// <summary>
  /// 敵を殺す
  /// </summary>
  public void Kill()
  {
    hp.Empty();
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
    SkillManager.Instance.StockExp(skillId, exp);
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

    // ステータス系パラメータの準備
    hp      = new RangedFloat(0);
    power   = 0;
    attrA   = new Flag32();
    attrW   = new Flag32();
    attrN   = new Flag32();
    skillId = SkillId.Undefined;
    exp     = 0;

    state = new StateMachine<T>();
  }

  // Update is called once per frame
  void Update()
  {
    state.Update();
  }
}