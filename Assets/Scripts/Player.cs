using UnityEngine;

public class Player : MonoBehaviour
{
  private enum State
  {
    Idle,
    Usual,
    Invincible,
    Dead,
  }

  private float timer = 0;
  private Vector3 velocity = Vector3.zero;
  private Vector3 targetVelocity = Vector3.zero;

  private RangedFloat _hp;

  private SpriteRenderer spriteRenderer;

  new public SphereCollider collider { get; private set; }

  private Collider battleLocation = null;

  public Vector3 VisualPosition {
    get { return transform.position + collider.center; }
  }

  public bool IsDead {
    get { return _hp.IsEmpty; }
  }

  private StateMachine<State> state = new StateMachine<State>();

  public void Respawn()
  {
    _hp.Full();
    state.SetState(State.Idle);
    SyncHpToHudHpGauge();
  }

  public void SetStateUsual()
  {
    state.SetState(State.Usual);
  }

  void Awake()
  {
    collider = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    _hp = new RangedFloat(10f);

    state.Add(State.Idle);
    state.Add(State.Usual, EnterStateUsual, UpdateStateUsual);
    state.Add(State.Invincible, EnterStateInvisible, UpdateStateInvisible);
    state.Add(State.Dead);
    state.SetState(State.Idle);
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    SyncHpToHudHpGauge();
  }

  public void takeDamage(float value)
  {
    if (state.StateKey != State.Usual) {
      return;
    }

    _hp.Now -= value;

    SyncHpToHudHpGauge();

    if (_hp.IsEmpty) {
      state.SetState(State.Dead);
    } else {
      state.SetState(State.Invincible);
    }
  }

  private void Update()
  {
    state.Update();

    var colliders = Physics.OverlapSphere(transform.position, collider.radius, LayerMask.GetMask("BattleLocation"));

    if (0 < colliders.Length) {
      battleLocation = colliders[0];
    } else {
      battleLocation = null;
    }

    if (battleLocation != null) 
    {
      var data = battleLocation.transform.Find("WaveData");

      for(int i = 0; true; ++i) 
      {
        var waves = data.transform.Find($"Waves{i}");

        if (waves == null) {
          break;
        }

        var settings = waves.GetComponentsInChildren<EnemyWaveSettings>();

        if (settings != null)
        {
          Debug.Log($"[Player] {settings[0].EnemyId}");
        }
      }
    }
  }

  private void EnterStateUsual()
  {
    spriteRenderer.material.color = Color.white;
  }

  private void UpdateStateUsual()
  {
    Vector3 v = Vector3.zero;

    if (Input.GetKey(KeyCode.LeftArrow)) {
      v.x = -5f;
    }

    if (Input.GetKey(KeyCode.RightArrow)) {
      v.x = 5f;
    }

    if (Input.GetKey(KeyCode.UpArrow)) {
      v.z = 5f;
    }

    if (Input.GetKey(KeyCode.DownArrow)) {
      v.z = -5f;
    }

    targetVelocity = v;

    velocity = Vector3.Lerp(velocity, targetVelocity, 0.01f);

    transform.position += velocity * TimeSystem.DeltaTime;

    SyncCameraPosition();
  }

  private void SyncCameraPosition()
  {
    var p = transform.position;
    p.y = 7.5f;
    p.z -= 15f;
    
    Camera.main.transform.position = p;
  }

  private void EnterStateInvisible()
  {
    timer = 0.2f;
    var c = spriteRenderer.material.color;
    c.a = 0.3f;
    spriteRenderer.material.color = c;
  }

  private void UpdateStateInvisible()
  {
    Vector3 v = Vector3.zero;

    if (Input.GetKey(KeyCode.LeftArrow)) {
      v.x = -5f;
    }

    if (Input.GetKey(KeyCode.RightArrow)) {
      v.x = 5f;
    }

    if (Input.GetKey(KeyCode.UpArrow)) {
      v.z = 5f;
    }

    if (Input.GetKey(KeyCode.DownArrow)) {
      v.z = -5f;
    }

    targetVelocity = v;

    velocity = Vector3.Lerp(velocity, targetVelocity, 0.01f);

    transform.position += velocity * TimeSystem.DeltaTime;

    if (timer < 0) {
      state.SetState(State.Usual);
      return;
    }

    timer -= TimeSystem.DeltaTime;

    SyncCameraPosition();
  }

  private void SyncHpToHudHpGauge()
  {
    UIManager.Instance.HUD.SetHpGauge((int)_hp.Now, _hp.Rate);
  }
}
