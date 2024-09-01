using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  [SerializeField]
  private HpGauge hpGauge;

  [SerializeField]
  private Text _PhaseText;

  [SerializeField]
  private SkillSlots skillSlots;


  public HpGauge HpGauge { 
    get { return hpGauge; } 
  }

  public SkillSlots SkillSlots {
    get { return skillSlots; }
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
