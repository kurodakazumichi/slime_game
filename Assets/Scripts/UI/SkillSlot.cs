using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MyMonoBehaviour
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

  private ISkill _skill;

  public void SetSkill(ISkill skill)
  {
    _skill = skill;
  }
  
  public void Charge()
  {
    if (_skill == null) { 
      SetActive(false);
      return;
    }
    SetActive(true);
    _state.SetState(State.Charge);
  }

  public void Idle()
  {
    _state.SetState(State.Idle);
  }

  protected override void MyAwake()
  {
    _state = new StateMachine<State>();
    
    _state.Add(State.Idle, EnterIdle);
    _state.Add(State.Charge, EnterCharge, UpdateCharge);
    _state.Add(State.Fire, EnterFire, UpdateFire);
    _state.SetState(State.Idle);

    _overlayImage.fillAmount = 0;
  }

  private void EnterIdle()
  {
    _recastTime = 0f;
    _timer = 0f;
    _overlayImage.fillAmount = 0;
  }

  private void EnterCharge()
  {
    _timer      = _skill.RecastTime;
    _recastTime = _skill.RecastTime;
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
