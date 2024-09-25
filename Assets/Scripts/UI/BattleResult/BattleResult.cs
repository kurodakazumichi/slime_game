using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
  public class BattleResult : MyUIBehaviour
  {
    [SerializeField]
    private Text title;

    [SerializeField]
    private GameObject skillRecordPrefab;

    private List<SkillRecord> skillRecordList = new();

    public void Win()
    {
      title.text = "勝利";
    }

    public void Lose()
    {
      title.text = "敗北";
    }

    public void Show()
    {
      SetActive(true);

      BattleLog.Instance.ScanSkillLog((info) => 
      {
        var record = Instantiate(skillRecordPrefab, CachedRectTransform).GetComponent<SkillRecord>();
        record.AnchoredPosition = new Vector2(-220f, 200 + (info.Index * 70));
        record.Show(info);
        skillRecordList.Add(record);
      });
    }

    public void Hide()
    {
      SetActive(false);

      foreach (var record in skillRecordList) {
        Destroy(record.gameObject);
      }
      skillRecordList.Clear();
    }

    protected override void MyAwake()
    {
      Hide();
    }
  }

}