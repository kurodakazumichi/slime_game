using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// 戦場の情報
/// </summary>
public struct BattleLocationInfo { 
  public string Name;
  public int Lv;
  public int TargetCount;
  public List<EnemyId> EnemyIds;
}

/// <summary>
/// [UI]戦場の情報を表示する看板
/// </summary>
public class BattleLocationBoard : MyMonoBehaviour
{
  //============================================================================
  // Inspector
  //============================================================================

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

  //============================================================================
  // Const
  //============================================================================
  const float ICON_SIZE    = 100;
  const float ICON_PADDING = 10;

  //============================================================================
  // Variables
  //============================================================================

  private IObjectPool<EnemyIcon> iconPool;

  private List<EnemyIcon> icons = new();

  //============================================================================
  // Properties
  //============================================================================


  public string LocationName {
    set { nameText.text = value; }
  }

  public int Lv {
    set { lvText.text = $"Lv {value}"; }
  }

  public int TargetCount {
    set { GoalText.text = $"目標：敵を {value} 匹倒す"; }
  }

  public bool IsVisible {
    set { SetActive(value); }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  public void Show(BattleLocationInfo info)
  {
    IsVisible     = true;
    nameText.text = info.Name;
    Lv            = info.Lv;
    TargetCount   = info.TargetCount;

    for(int i = 0, count = info.EnemyIds.Count; i < count; ++i) 
    {
      var id = info.EnemyIds[i];

      var icon = iconPool.Get();
      icon.SetSprite(IconManager.Instance.Enemy(id));
      icon.SetSize(ICON_SIZE, ICON_SIZE);

      icon.CachedRectTransform.anchoredPosition = CalcIconPosition(i, count);
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

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    IsVisible = false;

    iconPool = new LinkedPool<EnemyIcon>(
      () => Instantiate(iconPrefab, EnemyIconFolder).GetComponent<EnemyIcon>(),
      icon => icon.SetActive(true),
      icon => icon.SetActive(false),
      icon => Destroy(icon.gameObject)
    );
  }

  //----------------------------------------------------------------------------
  // Other
  //----------------------------------------------------------------------------

  /// <summary>
  /// 敵アイコンを配置する座標を計算する
  /// </summary>
  /// <param name="no">何番目に表示されるアイコンかを表すindex</param>
  /// <param name="max">最大で表示されるアイコンの数</param>
  /// <returns></returns>
  private Vector2 CalcIconPosition(int index, int max)
  {
    // iconの横幅をw、icon間の余白をpと置くと、アイコンがn個並ぶときに一番左にくるアイコンのx座標は
    // x = -(w+p)/2 * (n-1)で求められる
    const float w = ICON_SIZE;
    const float p = ICON_PADDING;
    int         n = max;
    var x = -(w-p)/2 * (n-1);

    // 起点となるx座標からicon個分ずらした座標を返す
    return new Vector2(x + ((w + p) * index), 0f);
  }
}
