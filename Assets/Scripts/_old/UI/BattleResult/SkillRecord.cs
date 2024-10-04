using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
  public class SkillRecord : MyUIBehaviour
  {
    [SerializeField]
    private SkillIcon uiIcon;

    [SerializeField]
    private Text uiNewIcon;

    [SerializeField]
    private Text uiExp;

    [SerializeField]
    private Text uiLevel;

    [SerializeField]
    private Text uiName;

    public void Show(SkillRecordInfo info)
    {
      uiIcon.SetSprite(IconManager.Instance.Skill(info.Config.Id));

      uiNewIcon.gameObject.SetActive(info.IsNew);
      uiExp.text = $"+{info.Exp}";
      uiLevel.text = $"{info.PrevLv} Å® {info.CrntLv}";
      uiName.text = info.Config.Name;
    }

    public void Hide()
    {
      SetActive(false);
    }
  }




}
