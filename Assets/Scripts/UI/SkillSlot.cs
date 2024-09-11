using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MyMonoBehaviour
{
  //============================================================================
  // Inspector
  //============================================================================

  [SerializeField]
  private Image uiOverlayImage;

  [SerializeField]
  private Image uiIcon;

  //============================================================================
  // Enum
  //============================================================================

  private enum State
  {
    Idle,
    Charge,
    Fire,
  }

  //============================================================================
  // Variables
  //============================================================================
  
  private StateMachine<State> state = new();
  private float recastTime = 0;
  private float timer = 0;
  private ISkill skill;

  //============================================================================
  // Properties
  //============================================================================

  private SpriteRenderer SpriteRenderer { get; set; }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  public void SetSkill(ISkill skill)
  {
    this.skill = skill;

    if (skill is null) {
      return;
    }

    uiIcon.sprite = IconManager.Instance.Skill(skill.Id);
  }

  public void Charge()
  {
    if (skill == null) {
      SetActive(false);
      return;
    }
    SetActive(true);
    state.SetState(State.Charge);
  }

  public void Idle()
  {
    state.SetState(State.Idle);
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    state.Add(State.Idle, EnterIdle);
    state.Add(State.Charge, EnterCharge, UpdateCharge);
    state.Add(State.Fire, EnterFire, UpdateFire);
    state.SetState(State.Idle);

    uiOverlayImage.fillAmount = 0;
  }

  void Update()
  {
    state.Update();
  }

  //----------------------------------------------------------------------------
  // State
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // Idle
  private void EnterIdle()
  {
    recastTime = 0f;
    timer = 0f;
    uiOverlayImage.fillAmount = 0;
  }

  //----------------------------------------------------------------------------
  // Charge
  private void EnterCharge()
  {
    timer      = skill.RecastTime;
    recastTime = skill.RecastTime;
    uiOverlayImage.fillAmount = 1f;
  }

  private void UpdateCharge()
  {
    uiOverlayImage.fillAmount = timer / recastTime;

    if (timer < 0) {
      state.SetState(State.Fire);
    }

    timer -= TimeSystem.Skill.DeltaTime;
  }

  //----------------------------------------------------------------------------
  // Fire
  private void EnterFire()
  {
    uiOverlayImage.fillAmount = 0;
  }

  private void UpdateFire()
  {
    skill.Fire();
    state.SetState(State.Charge);
  }

}
