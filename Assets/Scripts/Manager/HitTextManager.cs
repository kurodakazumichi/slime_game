using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HitTextManager : SingletonMonoBehaviour<HitTextManager>
{
  /// <summary>
  /// HitText‚Ìe‚Æ‚·‚éCanvas‚ğw’è‚·‚éB
  /// </summary>
  [SerializeField]
  private Canvas parent;

  [SerializeField]
  private GameObject hitTextPrefab;

  private LinkedPool<HitText> _pool;
  private List<HitText> _hitTexts;

  protected override void MyAwake()
  {
    HitText createFunc()
    {
      var t = Instantiate(hitTextPrefab).GetComponent<HitText>();
      t.transform.SetParent(parent.transform);
      return t;
    }

    _pool = new LinkedPool<HitText>(
      createFunc,
      t => t.gameObject.SetActive(true),
      t => t.gameObject.SetActive(false),
      t => Destroy(t.gameObject)
    );

    _hitTexts = new List<HitText>();
  }

  public HitText Get()
  {
    var t = _pool.Get();
    _hitTexts.Add(t);
    return t;
  }

  public void Release(HitText t)
  {
    _pool.Release(t);
    _hitTexts.Remove(t);
  }

  public void ReleaseAll()
  {
    _hitTexts.ForEach(t => _pool.Release(t));
  }

  public void Clear()
  {
    ReleaseAll();
    _hitTexts.Clear();
    _pool.Clear();
  }
}
