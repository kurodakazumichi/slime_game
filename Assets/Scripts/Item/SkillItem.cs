using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MyMonoBehaviour, ISkillItem
{
  public SkillId Id { get; private set; } = SkillId.Undefined;

  public int Exp { get; private set; } = 0;

  new private SphereCollider collider = null;
  private SpriteRenderer spriteRenderer = null;
  private Shadow shadow = null;
  public void Init(SkillId id, int exp)
  {
    Id = id;
    Exp = exp;
    shadow = ShadowManager.Instance.Get();
    shadow.SetOwner(this, collider.radius);

    spriteRenderer.sprite = IconManager.Instance.Skill(id);
  }

  protected override void MyAwake()
  {
    CachedTransform.rotation = Quaternion.Euler(45f, 0, 0);

    collider = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
  }

  private void Update()
  {
    var y = Mathf.Sin(Time.time*3f) * 0.1f;
    var p = Position;
    p.y = y;
    Position = p;
  }
  
  private void LateUpdate()
  {
    var a = PlayerManager.Instance.Position;
    var b = collider.transform.position;
    var r = PlayerManager.Instance.Collider.radius + collider.radius;

    if (CollisionUtil.IsCollideAxB(a, b, r)) {
      SkillManager.Instance.AddExp(Id, Exp);
      var text = HitTextManager.Instance.Get();
      text.ShowExp(Position, Exp);
      Reset();
      ItemManager.Instance.ReleaseSkillItem(this);
    }
  }

  public void Reset()
  {
    ShadowManager.Instance.Release(shadow);
    shadow = null;
    Exp = 0;
    Id = SkillId.Undefined;
    spriteRenderer.sprite = null;
  }
}
