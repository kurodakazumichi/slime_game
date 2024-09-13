using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
  private float damage;

  public void Init(float value)
  {
    damage = value;
  }

  public float Damage => damage;
}
