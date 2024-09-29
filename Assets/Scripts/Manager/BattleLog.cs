using System;
using System.Collections.Generic;
using MyGame.Master;

public struct SkillRecordInfo { 
  public int Index;
  public ISkillEntity Config;
  public int Exp;
  public int PrevLv;
  public int CrntLv;
  public bool IsNew;
}

public class BattleLog : SingletonMonoBehaviour<BattleLog>
{
  /// <summary>
  /// スキル経験値
  /// </summary>
  private Dictionary<int, int> exps = new();

  public void Reset()
  {
    exps.Clear();
  }

  public void AddExp(SkillId skillId, int exp)
  {
    if (exps.ContainsKey((int)skillId)) {
      exps[(int)skillId] += exp;
    } else {
      exps.Add((int)skillId, exp);
    }
  }

  public void ScanSkillLog(Action<SkillRecordInfo> action)
  {
    int index = 0;
    foreach (var item in exps) {

      var id  = (SkillId)item.Key;
      var exp = item.Value;

      var config = SkillMaster.FindById(id);

      var crntExp = SkillManager.Instance.GetExp(id);
      var prevExp = crntExp - exp;
      var crntLv = SkillManager.Instance.GetLevel(id);
      var prevLv = SkillUtil.CalcLevelBy(config, prevExp);
      var isNew  = (prevExp < 0);

      action?.Invoke(new SkillRecordInfo() {
        Index = index, 
        Config = config, 
        Exp = exp, 
        PrevLv = prevLv, 
        CrntLv = crntLv, 
        IsNew = isNew
      });
      index++;
    }
  }
}
