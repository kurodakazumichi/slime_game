using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
  private enum State
  {
    Start,
    Usual,
    GameOver,
  }

  private float _timer = 0;

  private StateMachine<State> _state;

  private void Awake()
  {
    _state = new StateMachine<State>();

    _state.Add(State.Start, EnterStart, UpdateStart);
    _state.Add(State.Usual, EnterUsual, UpdateUsual);
    _state.Add(State.GameOver, EnterGameOver, UpdateGameOver); 
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    _state.SetState(State.Start);

    

  }

  private void EnterStart()
  {
    UIManager.Instance.HUD.SetPhaseTextStart();
    EnemyManager.Instance.Clear();
    PlayerManager.Instance.RespawnPlayer();
    _timer = 0f;
  }

  private void UpdateStart()
  {
    const float START_TIME = 1f;

    _timer += TimeSystem.Scene.DeltaTime;

    if (START_TIME <= _timer) {
      UIManager.Instance.HUD.SetActivePhaseText(false);
      _state.SetState(State.Usual);
    }
  }

  private void EnterUsual()
  {
    PlayerManager.Instance.SetPlayerStateUsual();
  }

  private void UpdateUsual()
  {
    if (PlayerManager.Instance.PlayerIsDead) {
      _state.SetState(State.GameOver);
    }
  }

  private void EnterGameOver()
  {
    TimeSystem.Pause = true;
    UIManager.Instance.HUD.SetPhaseTextGameOver();
  }

  private void UpdateGameOver()
  {
    if (Input.GetKeyDown(KeyCode.Return)) {
      TimeSystem.Pause = false;
      _state.SetState(State.Start);
    }
  }

  // Update is called once per frame
  void Update()
  {
    _state.Update();
  }

  private void LateUpdate()
  {
    // ƒvƒŒƒCƒ„[‚ÌUŒ‚‚Æ“G‚ÌÕ“Ë
    CollisionManager.Instance.CollidePlayerAttackWithEnemy();
  }

  private void OnGUI()
  {

  }
}
