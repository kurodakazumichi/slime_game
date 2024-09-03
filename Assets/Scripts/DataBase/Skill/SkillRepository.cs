using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillRepository
{
  public static List<ISkillEntityRO> entities = new List<ISkillEntityRO>() {
    //             | ID                 | EXP | FirstRecastTime | LastRecastTime | FirstPower | LastPower | Attribute          | Name      |
    //             +--------------------+-----+-----------------+----------------+------------+-----------+--------------------+-----------|
    new SkillEntity(SkillId.NormalBullet, 30  , 1f              , 0.1f           , 1          , 10        , (int)Attribute.Non , "í èÌíe"),
  };
}
