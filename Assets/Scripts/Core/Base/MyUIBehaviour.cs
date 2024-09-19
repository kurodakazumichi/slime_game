using UnityEngine;

public abstract class MyUIBehaviour : MonoBehaviour
{
  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// Transformのキャッシュ(処理負荷軽減対策)
  /// </summary>
  public RectTransform CachedRectTransform { get; private set; }

  /// <summary>
  /// RectTransform.anchoredPositionのsyntax sugar
  /// </summary>
  public Vector2 AnchoredPosition {
    get { return CachedRectTransform.anchoredPosition; }
    set { CachedRectTransform.anchoredPosition = value; }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected void Awake()
  {
    if (CachedRectTransform == null) {
      CachedRectTransform = GetComponent<RectTransform>();
    }

    MyAwake();
  }

  protected virtual void MyAwake() { }

  //----------------------------------------------------------------------------
  // syntax sugar
  //----------------------------------------------------------------------------


  /// <summary>
  /// アクティブを設定する
  /// </summary>
  public void SetActive(bool isActive)
  {
    this.gameObject.SetActive(isActive);
  }
}
