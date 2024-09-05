using UnityEngine;

public interface ISkill
{
  SkillId Id { get; }
  int Lv { get; }
  float RecastTime { get; }
  int Power { get; }
  void Fire();
  void SetExp(int exp);
}

public class Skill : ISkill
{
  private const int MAX_LEVEL = 9;
  protected ISkillEntityRO _entity;

  public void Setup(ISkillEntityRO entity)
  {
    _entity = entity;
  }

  public int Lv {
    get; private set;
  }

  public SkillId Id {
    get { return _entity.Id; }
  }
  public float RecastTime {
    get; private set;
  }

  public int Power {
    get; private set;
  }

  public virtual void Fire()
  {
    var bullet = BulletManager.Instance.Get(Id);
    bullet.Fire(this);
  }

  public void SetExp(int exp)
  {
    Lv = CalcLevelBy(exp);
    RecastTime = CalcRecastTimeBy(Lv);
    Power = CalcPowerBy(Lv);

    Debug.Log($"Exp = {exp} Lv = {Lv} RT = {RecastTime} Power = {Power}");
  }

  private int CalcLevelBy(int exp)
  {
    for (int i = MAX_LEVEL; 0 <= i; --i) {

      if (GetNeedExp(i) <= exp) {
        return i;
      }
    }

    return 0;
  }

  private float CalcRecastTimeBy(int lv)
  {
    return Mathf.Lerp(_entity.FirstRecastTime, _entity.LastRecastTime, lv / (float)MAX_LEVEL);
  }

  private int CalcPowerBy(int lv)
  {
    return (int)Mathf.Lerp(_entity.FirstPower, _entity.LastPower, lv / (float)MAX_LEVEL);
  }

  private int GetNeedExp(int lv)
  {
    lv = Mathf.Clamp(lv, 0, MAX_LEVEL);
    return (int)Mathf.Lerp(0, _entity.MaxExp, (float)(lv) / MAX_LEVEL);
  }

}
