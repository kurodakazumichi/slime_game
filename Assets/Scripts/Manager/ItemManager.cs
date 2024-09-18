using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface ISkillItem: IMono
{
  void Init(SkillId id, int exp);
  SkillId Id { get; }
  int Exp { get; }
  void Reset();
}

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
  public void Load()
  {
    ResourceManager.Instance.Load<GameObject>("Item/SkillItem.prefab");
  }

  private LinkedList<ISkillItem> skillItems = new();
  private IObjectPool<ISkillItem> skillItemPool;

  protected override void MyAwake()
  {
    skillItemPool = new LinkedPool<ISkillItem>(
      () => MakeSkillItem(),
      (i) => i.SetActive(true),
      (i) => i.SetActive(false),
      (i) => Destroy(i.gameObject)
    );
  }

  public ISkillItem GetSkillItem()
  {
    var item = skillItemPool.Get();
    skillItems.AddLast(item);
    return item;
  }

  public void ReleaseSkillItem(ISkillItem item)
  {
    item.Reset();
    skillItems.Remove(item);
    skillItemPool.Release(item);
  }

  public void Clear()
  {
    foreach (var item in skillItems) {
      item.Reset();
      skillItemPool.Release(item);
    }
    skillItems.Clear();
    skillItemPool.Clear();
  }

  private ISkillItem MakeSkillItem()
  {
    var prefab = ResourceManager.Instance.GetCache<GameObject>("Item/SkillItem.prefab");
    return Instantiate(prefab).GetComponent<ISkillItem>();
  }
}
