using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShadowManager : SingletonMonoBehaviour<ShadowManager>
{
  private IObjectPool<Shadow> pool;

  public void Load()
  {
    ResourceManager.Instance.Load<GameObject>("Shadow/Shadow.prefab");
  }

  protected override void MyAwake()
  {
    pool = new LinkedPool<Shadow>(
      () => {
        var prefab = ResourceManager.Instance.GetCache<GameObject>("Shadow/Shadow.prefab");
        return Instantiate(prefab).GetComponent<Shadow>();
      },
      (o) => o.SetActive(true),
      (o) => o.SetActive(false),
      (o) => Destroy(o.gameObject)
    );
  }

  public Shadow Get() {
    return pool.Get();
  }

  public void Release(Shadow shadow)
  {
    pool.Release(shadow);
  }
}
