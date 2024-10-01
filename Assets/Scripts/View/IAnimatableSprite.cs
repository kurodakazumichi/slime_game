using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatableSprite
{
  SpriteRenderer Renderer { get; }
  Animator Animator { get; }
}