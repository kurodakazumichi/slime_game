using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
  [SerializeField]
  private Image _overlayImage;

  private enum State
  {
    Idle,
    Charge,
    Fire,
  }

  private float _recastTime = 0;
  private float _timer = 0;

  private StateMachine<State> _state;

  public RectTransform CacheRectTransform;

  private ISkill _skill;

  public void SetSkill(ISkill skill)
  {
    _skill = skill;
  }
  
  public void Charge()
  {
    _state.SetState(State.Charge);
  }

  private void Awake()
  {
    CacheRectTransform = GetComponent<RectTransform>();
    _recastTime = 1f;
    _state = new StateMachine<State>();
    _overlayImage.fillAmount = 0;

    _state.Add(State.Idle);
    _state.Add(State.Charge, EnterCharge, UpdateCharge);
    _state.Add(State.Fire, EnterFire, UpdateFire);
    _state.SetState(State.Idle);
  }

  private void EnterCharge()
  {
    _timer = _skill.RecastTime;
    _overlayImage.fillAmount = 1f;
  }

  private void UpdateCharge()
  {
    _overlayImage.fillAmount = _timer / _recastTime;

    if (_timer < 0) {
      _state.SetState(State.Fire);
    }

    _timer -= TimeSystem.Skill.DeltaTime;
  }

  private void EnterFire()
  {
    _overlayImage.fillAmount = 0;
  }

  private void UpdateFire()
  {
    _skill.Fire();
    _state.SetState(State.Charge);
  }

  // Update is called once per frame
  void Update()
  {
    _state.Update();
  }
}
