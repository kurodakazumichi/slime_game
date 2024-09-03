using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfo : MyMonoBehaviour
{
  [SerializeField]
  private Text nameText;

  [SerializeField]
  private Text lvText;

  public string LocationName {
    set { nameText.text = value; }
  }

  public int Lv {
    set { lvText.text = $"Lv {value}"; }
  }

  public bool IsVisible {
    set { SetActive(value); }
  }


  protected override void MyAwake()
  {
    IsVisible = false;
  }
}
