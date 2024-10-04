using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingCount : MyUIBehaviour
{
  [SerializeField]
  private Text uiCountText;

  public int Value {
    set { uiCountText.text = value.ToString(); }
  }
}
