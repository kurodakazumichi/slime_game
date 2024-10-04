using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// クリアゲージ
  /// </summary>
  [SerializeField]
  private SimpleGauge clearGauge;

  /// <summary>
  /// 倒さないといけない敵の残りの数
  /// </summary>
  [SerializeField]
  private RemainingCount count;

  /// <summary>
  /// プレイヤーのHPゲージ
  /// </summary>
  [SerializeField]
  private SimpleGauge hpGauge;

  /// <summary>
  /// スキルスロット
  /// </summary>
  [SerializeField]
  private SkillSlots skillSlots;



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
