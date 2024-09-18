using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitText : MonoBehaviour
{
  private enum State
  {
    Idle,
    Display,
    Hidden,
  }

  private Text _text;
  private Outline _outline;
  private StateMachine<State> _state;
  private float _timer = 0;
  private void Awake()
  {
    _text = GetComponent<Text>();
    _outline = GetComponent<Outline>();
    _state = new StateMachine<State>();

    _state.Add(State.Idle, EnterIdle);
    _state.Add(State.Display, EnterDisplay, UpdateDisplay);
    _state.Add(State.Hidden, null, UpdateHidden);
    _state.SetState(State.Idle);
  }

  public void SetDisplay(Vector3 position, int value)
  {
    _text.text = value.ToString();
    _text.transform.position = position;
    _text.color = new Color(1f, 0.38f, 0);
    _outline.effectColor = new Color(0.73f, 0, 0, 0.5f);
    _state.SetState(State.Display);
  }

  public void ShowExp(Vector3 position, int exp)
  {
    _text.text = $"+{exp.ToString()}";
    _text.transform.position = position;
    _text.color = new Color(0.56f, 0.96f, 1f);
    _outline.effectColor = new Color(0.17f, 0.26f, 1f, 0.5f);
    _state.SetState(State.Display);
  }

  private void EnterIdle()
  {
    _text.text = "";
  }

  private void EnterDisplay()
  {
    _timer = 0.5f;
  }

  private void UpdateDisplay()
  {
    _timer -= TimeSystem.UI.DeltaTime;

    if (_timer < 0) {
      _state.SetState(State.Hidden);
    }

    transform.position += Vector3.up * TimeSystem.UI.DeltaTime;
  }

  private void UpdateHidden()
  {
    _state.SetState(State.Idle);
    HitTextManager.Instance.Release(this);
  }

  void Update()
  {
    _state.Update();
  }
}
