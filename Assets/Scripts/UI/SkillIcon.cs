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

  [SerializeField]
  private GameObject uiEquipIcon;

  public bool IsSelected {
    set {
      uiBackground.color = (value)? Color.yellow : Color.white;
    }
  }

  public bool IsEquipped {
    set {
      uiEquipIcon.SetActive(value);
    }
  }

  public void SetSprite(Sprite sprite)
  {
    uiIconImage.sprite = sprite;
    uiIconImage.color = (sprite is null)? new Color(0, 0, 0, 0) : Color.white;
  }

  public void SetSize(float x, float y)
  {
    CachedRectTransform.sizeDelta = new Vector2(x, y);
  }
}
