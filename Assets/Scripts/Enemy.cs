using UnityEngine;

public interface IEnemy
{
  public EnemyId Id { get; }
  public void TakeDamage(AttackStatus p);
}

public class Enemy : MyMonoBehaviour, IEnemy
{
  //============================================================================
  // Variables
  //============================================================================
  private EnemyId id;
  private RangedFloat hp;
  private EnemyWave ownerWave;
  new private SphereCollider collider;

  //============================================================================
  // Properities
  //============================================================================
  public EnemyId Id { get { return id; } }
  private bool destroyFlag {
    get { return hp.Now <= 0f; }
  }
  public Vector3 VisualPosition {
    get { return transform.position + collider.center; }
  }
  public SphereCollider Collider { get { return collider; } }
  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  public void Init(EnemyId id) 
  {
    this.id = id;

    var master = EnemyMaster.FindById(id);
    hp.Init(master.HP);

  }

  
  public void SetBelongsTo(EnemyWave wave) { ownerWave = wave; }

  public void TakeDamage(AttackStatus p)
  {
    Debug.Log($"[Enemy] AttackParam.power = {p.Power}");
    HitTextManager.Instance.Get().SetDisplay(VisualPosition, (int)p.Power);

    hp.Now -= p.Power;
  }







  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    collider = GetComponent<SphereCollider>();
    hp = new RangedFloat(0);
  }

  // Update is called once per frame
  void Update()
  {
    // Waveに終了フラグがたっていたら敵は即座に死ぬ
    if (ownerWave != null && ownerWave.IsTerminating) {
      hp.Empty();
    }

    if (destroyFlag) {
      if (ownerWave != null) {
        ownerWave.Release(this);
      }
      else {
        EnemyManager.Instance.Release(this);
      }

      SkillManager.Instance.StockExp(SkillId.NormalBullet, 1);
      return;
    }

    var v = PlayerManager.Instance.PlayerVisualPosition - transform.position;
    transform.position += v.normalized * TimeSystem.DeltaTime;

    var d1 = (PlayerManager.Instance.PlayerVisualPosition - transform.position).sqrMagnitude;
    var d2 = Collider.radius * Collider.radius;

    if (d1 < d2) {
      PlayerManager.Instance.takeDamage(1f);
    }

  }

  private void OnEnable()
  {
    hp.Now = 2;
  }


}
