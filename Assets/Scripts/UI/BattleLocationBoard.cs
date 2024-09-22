using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// íê‚Ìî•ñ
/// </summary>
public struct BattleLocationInfo { 
  public string Name;
  public int Lv;
  public int TargetCount;
  public List<EnemyId> EnemyIds;
}

/// <summary>
/// [UI]íê‚Ìî•ñ‚ğ•\¦‚·‚éŠÅ”Â
/// </summary>
public class BattleLocationBoard : MyMonoBehaviour
{
  [SerializeField]
  private Text nameText;

  [SerializeField]
  private Text lvText;

  [SerializeField]
  private Text GoalText;

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

  public int TargetCount {
    set { GoalText.text = $"–Ú•WF“G‚ğ {value} •C“|‚·"; }
  }

  public bool IsVisible {
    set { SetActive(value); }
  }

  public void Show(BattleLocationInfo info)
  {
    IsVisible = true;
    nameText.text = info.Name;
    Lv = info.Lv;
    TargetCount = info.TargetCount;

    var x = -55 * (info.EnemyIds.Count - 1);

    for(int i = 0, count = info.EnemyIds.Count; i < count; ++i) 
    {
      var id = info.EnemyIds[i];

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
