using UnityEngine;

public interface IEnemy
{
  public EnemyId Id { get; }
  public void TakeDamage(AttackStatus p);
}

public class Enemy : MonoBehaviour, IEnemy
{
  public void Init(EnemyId id) { 
    this.id = id;
  }

  private EnemyWave _wave;
  public void SetBelongsTo(EnemyWave wave) { _wave = wave; }

  public EnemyId Id { get { return id; } }
  private EnemyId id;
  private float hp = 2;
  private bool destroyFlag { 
    get { return hp <= 0f;} 
  }
  private SphereCollider _collider;

  public SphereCollider Collider { get { return _collider; } }

  public Vector3 VisualPosition {
    get { return transform.position + _collider.center; }
  }

  public void TakeDamage(AttackStatus p)
  {
    Debug.Log($"[Enemy] AttackParam.power = {p.Power}");
    HitTextManager.Instance.Get().SetDisplay(VisualPosition, (int)p.Power);

    hp -= p.Power;
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    _collider = GetComponent<SphereCollider>(); 
  }

  private void OnEnable()
  {
    hp = 2;
  }

  // Update is called once per frame
  void Update()
  {
    // Wave‚ÉI—¹ƒtƒ‰ƒO‚ª‚½‚Á‚Ä‚¢‚½‚ç“G‚Í‘¦À‚É€‚Ê
    if (_wave != null && _wave.IsTerminating) {
      hp = 0;
    }

    if (destroyFlag) {
      if (_wave != null) {
        _wave.Release(this);
      } else {
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
}
