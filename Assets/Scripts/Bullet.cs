using UnityEngine;

public class Bullet : MonoBehaviour
{
  private enum State
  {
    Idle,
    Usual,
    Release,
  }

  new public SphereCollider collider {  get; private set; }

  private Vector3 velocity;

  private StateMachine<State> state;

  private int _power = 0;

  public void Fire(ISkill skill)
  {
    var p = PlayerManager.Instance.PlayerVisualPosition;
    transform.position = p;

    var e = EnemyManager.Instance.FindNearestEnemy(p);

    var v = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * Vector3.forward;

    if (e != null) {
      v = (e.transform.position - p).normalized;
    }

    this.velocity = v;

    this._power = skill.Power;
    state.SetState(State.Usual);
  }

  public void Attack(Enemy enemy)
  {
    var p = new AttackParams();
    p.Power = _power;
    enemy.takeDamage(p);
    state.SetState(State.Release);
  }

  private void Awake()
  {
    collider     = GetComponent<SphereCollider>();
    state = new StateMachine<State>();

    state.Add(State.Idle);
    state.Add(State.Usual, null, UpdateUsual);
    state.Add(State.Release, null, UpdateRelease);
    state.SetState(State.Usual);
  }

  private void UpdateUsual()
  {
    var p = transform.position;
    p += velocity * TimeSystem.DeltaTime;
    transform.position = p;

    var r = transform.rotation;
    transform.rotation = r * Quaternion.AngleAxis(720f * TimeSystem.DeltaTime, Vector3.forward);

    // ‰æ–ÊŠO‚É‚¢‚Á‚½‚çRelease
    if (IsOffScreen(transform.position)) {
      //BulletManager.Instance.Release(this);
    }
  }

  private void UpdateRelease()
  {
    BulletManager.Instance.Release(this);
    state.SetState(State.Idle);
  }

  // Update is called once per frame
  void Update()
  {
    state.Update();
  }

  private bool IsOffScreen(Vector3 position)
  {
    var cameraPos = Camera.main.transform.position;
    cameraPos.z = position.z;

    var d = cameraPos - position;

    return (8f < Mathf.Abs(d.x) || 5f < Mathf.Abs(d.y));
  }
}
