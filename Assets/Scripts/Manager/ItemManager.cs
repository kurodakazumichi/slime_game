using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface ISkillItem: IMono
{
  void Setup(SkillId id, int exp, Vector3 position);
  void Reset();
  void Move(Vector3 position, float time);
}

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
  public void Load()
  {
    ResourceManager.Instance.Load<GameObject>("Item/SkillItem.prefab");
  }

  private LinkedList<ISkillItem> skillItems = new();
  private IObjectPool<ISkillItem> skillItemPool;

  public int SkillItemCount => skillItems.Count;

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

  public void Collect(Vector3 position)
  {
    foreach (var item in skillItems) {
      var time = (position - item.Position).magnitude * 0.1f;
      item.Move(position, time);
    }
  }

  private ISkillItem MakeSkillItem()
  {
    var prefab = ResourceManager.Instance.GetCache<GameObject>("Item/SkillItem.prefab");
    return Instantiate(prefab).GetComponent<ISkillItem>();
  }
}
