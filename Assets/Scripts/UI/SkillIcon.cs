using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MyUIBehaviour
{
  //============================================================================
  // Inspector
  //============================================================================
  [SerializeField]
  private Image uiBackground;

  [SerializeField]
  private Image uiIconImage;

  public bool IsSelected {
    set {
      uiBackground.color = (value)? Color.yellow : Color.white;
    }
  }

  public void SetSprite(Sprite sprite)
  {
    uiIconImage.sprite = sprite;
  }

  public void SetSize(float x, float y)
  {
    CachedRectTransform.sizeDelta = new Vector2(x, y);
  }
}
