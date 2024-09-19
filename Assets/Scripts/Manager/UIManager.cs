using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
  [SerializeField]
  private HUD hud;

  [SerializeField]
  private BattleInfo battleInfo;

  [SerializeField]
  private Toaster toaster;

  public HUD HUD { get { return hud; } }

  public BattleInfo BattleInfo { get { return battleInfo; } }

  public Toaster Toaster { get { return toaster; } }
}
