using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollideable
{
  T GetCollider<T>() where T : Collider;
}
