using UnityEngine;

/// <summary>
/// UnityのMonoBehaviourラッパー
/// </summary>
public class MyMonoBehaviour : MonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// Transformのキャッシュ(処理負荷軽減対策)
  /// </summary>
  public Transform CacheTransform {  get; private set; }

  /// <summary>
  /// RectTransformのキャッシュ(処理負荷軽減対策)
  /// </summary>
  public RectTransform CacheRectTransform { get; private set; }
  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 自信がアクティブかどうか
  /// </summary>
  public bool IsActiveSelf => this.gameObject.activeSelf;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected void Awake()
  {
    if (CacheTransform == null) {
      CacheTransform = this.transform;
    }

    if (CacheRectTransform == null) {
      CacheRectTransform = GetComponent<RectTransform>();
    }

    MyAwake();
  }

  protected virtual void MyAwake() { }

  //----------------------------------------------------------------------------
  // syntax sugar
  //----------------------------------------------------------------------------
  /// <summary>
  /// 親を設定する
  /// </summary>
  public void SetParent(Transform parent, bool worldPositionStays = true)
  {
    this.CacheTransform.SetParent(parent, worldPositionStays);
  }

  /// <summary>
  /// アクティブを設定する
  /// </summary>
  public void SetActive(bool isActive)
  {
    this.gameObject.SetActive(isActive);
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public virtual void OnDebug()
  {

  }

#endif

}
