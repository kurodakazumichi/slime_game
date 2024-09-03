using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
  [SerializeField]
  private HUD hud;

  [SerializeField]
  private BattleInfo battleInfo;

  public HUD HUD { get { return hud; } }

  public BattleInfo BattleInfo { get { return battleInfo; } }
}
