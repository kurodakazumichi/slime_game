using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  [SerializeField]
  private SimpleGauge clearGauge;

  [SerializeField]
  private SimpleGauge hpGauge;

  [SerializeField]
  private SkillSlots skillSlots;

  [SerializeField]
  private RemainingCount count;

  public SkillSlots SkillSlots {
    get { return skillSlots; }
  }

  public bool IsVisibleClearGauge {
    get {
      return clearGauge.gameObject.activeSelf;
    }
    set {
      clearGauge.SetActive(value);
      count.SetActive(value);
    }
  }

  public bool IsVisibleHpGauge {
    get { return hpGauge.gameObject.activeSelf; }
    set { hpGauge.SetActive(value); }
  }

  /// <summary>
  /// クリアゲージを更新
  /// </summary>
  public void UpdateClearGauge(int value, float rate)
  {
    clearGauge.Rate = rate;
    count.Value = value;
  }

  public void UpdateHpGauge(float rate)
  {
    hpGauge.Rate = rate;
  }
}
