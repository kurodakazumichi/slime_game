using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : SingletonMonoBehaviour<IconManager>
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
}
