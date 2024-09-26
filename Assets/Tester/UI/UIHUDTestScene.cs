using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UITester
{
  public class UIHUDTestScene : MonoBehaviour
  {
    public HUD HUD;

    public int RemainingCount = 10;

    [Range(0, 1f)]
    public float ClearGaugeRate = 0.5f;

    [Range(0, 1f)]
    public float HpGaugeRate = 0.5f;

    public bool IsVisibleHpGauge = true;

    public bool IsVisibleClearGauge = true;

    
    private void Update()
    {
      HUD.IsVisibleHpGauge = IsVisibleHpGauge;

      if (IsVisibleHpGauge) {
        HUD.UpdateHpGauge(HpGaugeRate);
      }
      

      HUD.IsVisibleClearGauge = IsVisibleClearGauge;

      if (IsVisibleClearGauge) {
        HUD.UpdateClearGauge(RemainingCount, ClearGaugeRate);
      }
      


    }
  }
}

