using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
  [SerializeField]
  private HUD hud;

  [SerializeField]
  private BattleLocationBoard battleLocationBoard;

  [SerializeField]
  private Toaster toaster;

  [SerializeField]
  private MyGame.UI.BattleResult result;

  [SerializeField]
  private MyGame.UI.SkillSetting skillSetting;

  public HUD HUD { get { return hud; } }

  public BattleLocationBoard BattleLocationBoard { get { return battleLocationBoard; } }

  public Toaster Toaster { get { return toaster; } }

  public MyGame.UI.BattleResult Result => result;

  public MyGame.UI.SkillSetting SkillSetting => skillSetting;
}
