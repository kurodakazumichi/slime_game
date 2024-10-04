using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MyUIBehaviour
{
  //============================================================================
  // Inspector
  //============================================================================
  [SerializeField]
  private Image uiIconImage;

  public void SetSprite(Sprite sprite)
  {
    uiIconImage.sprite = sprite;
  }

  public void SetSize(float x, float y)
  {
    CachedRectTransform.sizeDelta = new Vector2(x, y);
  }
}
