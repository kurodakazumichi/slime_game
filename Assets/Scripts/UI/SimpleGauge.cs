using UnityEngine;
using UnityEngine.UI;

public class SimpleGauge : MyUIBehaviour
{
  [SerializeField]
  private Image fill;

  public float Rate {
    get { return fill.fillAmount; }
    set { fill.fillAmount = value; }
  }
}
