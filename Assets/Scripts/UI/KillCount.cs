using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MyMonoBehaviour
{
  [SerializeField]
  private Text uiCountText;

  private int count = 0;

  public void CountUp()
  {
    ++count;
    Count = count;
  }

  public void Reset()
  {
    count = 0;
    Count = 0;
  }

  public int Count {
    set {
      uiCountText.text = $"Å~ {value}";
    }
  }
}
