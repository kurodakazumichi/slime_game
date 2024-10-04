using UnityEngine;

namespace MyGame.View
{
  public class PlayerView : MyMonoBehaviour, IAnimatableSprite, ICollideable
  {
    //=========================================================================
    // Inspector
    //=========================================================================
    [SerializeField]
    private SphereCollider sphereCollider;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    //=========================================================================
    // Properties
    //=========================================================================
    public SpriteRenderer Renderer => spriteRenderer;

    public Animator Animator => animator;

    public T GetCollider<T>() where T : Collider{
      return sphereCollider as T;
    }
  }
}
