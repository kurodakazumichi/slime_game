using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
  public class BattleResult : MyUIBehaviour
  {
    [SerializeField]
    private Text title;

    public void Show()
    {
      SetActive(true);
    }

    public void Hide()
    {
      SetActive(false);
    }

    protected override void MyAwake()
    {
      Hide();
    }
  }

}