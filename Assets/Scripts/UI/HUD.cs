using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  [SerializeField]
  private Image _HpGaugefillRect;

  [SerializeField]
  private Text _HpText;

  [SerializeField]
  private Text _PhaseText;

  [SerializeField]
  private SkillSlotManager _skillSlotManager;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    _HpGaugefillRect.fillAmount = 0.5f;
    _HpText.text = 10.ToString();
  }

  public void SetHpGauge(int hp, float rate)
  {
    _HpText.text = hp.ToString();
    _HpGaugefillRect.fillAmount = Mathf.Clamp(rate, 0f, 1f);
  }

  public void SetPhaseTextStart()
  {
    SetActivePhaseText(true);
    _PhaseText.text = "START";
  }

  public void SetPhaseTextGameOver()
  {
    SetActivePhaseText(true);
    _PhaseText.text = "GAME OVER";
  }

  public void SetActivePhaseText(bool value)
  {
    _PhaseText.gameObject.SetActive(value);
  }
}
