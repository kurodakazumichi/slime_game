using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MyMonoBehaviour
{
  private MyMonoBehaviour owner;

  public void SetOwner(MyMonoBehaviour owner, float size)
  {
    this.owner = owner;
    CachedTransform.localScale = Vector3.one * size;
    SyncPosition();
  }

  private void Update()
  {
    if (owner is null) {
      return;
    }

    SyncPosition();
  }

  private void SyncPosition()
  {
    if (owner is null) {
      return;
    }

    var p = owner.Position;
    p.y = 0;
    CachedTransform.position = p;
  }
}
