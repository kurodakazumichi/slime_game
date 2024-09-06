using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MyMonoBehaviour
{
  public abstract void TakeDamage(AttackStatus p);
}
