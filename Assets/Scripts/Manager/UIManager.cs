using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
  [SerializeField]
  private HUD _hud;

  public HUD HUD { get { return _hud; } }


}
