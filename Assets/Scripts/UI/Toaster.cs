using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Toaster : MyMonoBehaviour
{
  //============================================================================
  // Inspector Variables
  //============================================================================
  [SerializeField]
  private GameObject toastPrefab;

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// トースト
  /// </summary>
  private Toast toast = null;

  /// <summary>
  /// メッセージリスト
  /// </summary>
  private LinkedList<string> messages = new();

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  public void Bake(string message)
  {
    messages.AddLast(message);
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    toast = Instantiate(toastPrefab).GetComponent<Toast>();
    toast.SetParent(CachedRectTransform);
  }

  private void Update()
  {
    if (!toast.IsIdle || messages.Count <= 0) {
      return;
    }

    var msg = messages.First<string>();
    messages.RemoveFirst();
    toast.Show(msg, new Vector2(0, -70), Vector2.zero);
  }
}
