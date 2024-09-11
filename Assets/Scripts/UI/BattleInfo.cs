using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BattleInfo : MyMonoBehaviour
{
  [SerializeField]
  private Text nameText;

  [SerializeField]
  private Text lvText;

  [SerializeField]
  private RectTransform EnemyIconFolder;

  [SerializeField]
  private GameObject iconPrefab;

  private IObjectPool<Icon> iconPool;

  private List<Icon> icons = new();

  public string LocationName {
    set { nameText.text = value; }
  }

  public int Lv {
    set { lvText.text = $"Lv {value}"; }
  }

  public bool IsVisible {
    set { SetActive(value); }
  }

  public void Show(string name, int lv, List<EnemyId> ids)
  {
    IsVisible = true;
    nameText.text = name;
    lvText.text = $"Lv {lv}";

    var x = -55 * (ids.Count - 1);

    for(int i = 0, count = ids.Count; i < count; ++i) 
    {
      var id = ids[i];

      var icon = iconPool.Get();
      icon.SetSprite(IconManager.Instance.Enemy(id));
      icon.SetSize(100, 100);

      icon.CachedRectTransform.anchoredPosition = new Vector2(x + (110*i), 0);

      icons.Add(icon);
    }

  }

  public void Hide()
  {
    IsVisible = false;

    foreach(var icon in icons) {
      iconPool.Release(icon);
    }

    icons.Clear();
  }


  protected override void MyAwake()
  {
    IsVisible = false;

    iconPool = new LinkedPool<Icon>(
      () => Instantiate(iconPrefab, EnemyIconFolder).GetComponent<Icon>(),
      icon => icon.SetActive(true),
      icon => icon.SetActive(false),
      icon => Destroy(icon.gameObject)
    );
  }
}
