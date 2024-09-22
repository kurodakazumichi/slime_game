using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
  [SerializeField]
  private HUD hud;

  [SerializeField]
  private BattleLocationBoard battleLocationBoard;

  [SerializeField]
  private Toaster toaster;

  public HUD HUD { get { return hud; } }

  public BattleLocationBoard BattleLocationBoard { get { return battleLocationBoard; } }

  public Toaster Toaster { get { return toaster; } }
}
