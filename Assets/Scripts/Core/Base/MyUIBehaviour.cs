using UnityEngine;

public abstract class MyUIBehaviour : MonoBehaviour
{
  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// Transform�̃L���b�V��(�������׌y���΍�)
  /// </summary>
  public RectTransform CachedRectTransform { get; private set; }

  /// <summary>
  /// RectTransform.anchoredPosition��syntax sugar
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
  /// �A�N�e�B�u��ݒ肷��
  /// </summary>
  public void SetActive(bool isActive)
  {
    this.gameObject.SetActive(isActive);
  }
}
