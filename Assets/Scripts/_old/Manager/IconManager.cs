using UnityEngine;
using MyGame.Master;
using MyGame.Core.System;

public class IconManager : SingletonMonoBehaviour<IconManager>
#if _DEBUG
  ,IDebugable
#endif
{
  public void Load()
  {
    ResourceManager.Instance.LoadSprites("Icon/Enemies.png");
    ResourceManager.Instance.LoadSprites("Icon/Bullets.png");
  }


  public Sprite Skill(SkillId id)
  {
    var index = SkillMaster.FindById(id).IconNo;
    var sprites = ResourceManager.Instance.GetSpritesCache("Icon/Bullets.png");
    return sprites[index];
  }

  public Sprite Enemy(EnemyId id)
  {
    var index = EnemyMaster.FindById(id).No;
    var sprites = ResourceManager.Instance.GetSpritesCache("Icon/Enemies.png");
    return sprites[index];
  }
}
