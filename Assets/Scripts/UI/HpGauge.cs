using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI HPゲージクラス
/// </summary>
public class HpGauge : MyMonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  [SerializeField]
  private Image fill;

  [SerializeField]
  private Text hpText;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  public void Set(float hp, float rate)
  {
    fill.fillAmount = rate;
    hpText.text = ((int)hp).ToString();
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    base.MyAwake();
    fill.fillAmount = 1f;
    hpText.text     = "0";
  }

}
